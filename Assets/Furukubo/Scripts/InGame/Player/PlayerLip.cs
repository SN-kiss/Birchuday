using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of LipControler)
    /// </summary>
    public class PlayerLip : MonoBehaviour, ILip, ILipAttractTarget
    {
        [Header("Parameters")]
        [SerializeField, Range(-1f, 1f)] private float _attractableRangeThreshoud;
        [SerializeField] private float _attractedCancelTime;
        [SerializeField] private float _attractedCoolTime;
        [SerializeField] private float _pullBodyPowerCoef;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _lipLengthMax;
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private PlayerBodyMove _bodyMove;
        [SerializeField] private PlayerBodyHealth _bodyHealth;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Transform _lipDefaultPointTr;
        [SerializeField] private Collider2D _ignoreCol;
        [SerializeField] private GameObject _objLipAttracter;
        [SerializeField] private KissConnector _prefabKissConnector;

        public MagneticType MagneticType => _selfMagneticType;
        public float LipLengthMax => _lipLengthMax;
        public bool IsKissableNow => CurrentState != PlayerLipState.Attaching && CurrentState != PlayerLipState.Dead;
        private Vector2 LipPositionDefault => _lipDefaultPointTr.position;

        private ILipAttachTarget _target;
        private Vector2 _attachedPoint;
        private float _attachedRotation;
        private float _attractedCancelTimeCounter;
        private float _attractCoolTimeCount;

        public PlayerLipState CurrentState { get; private set; }

        private bool LipKinematic
        {
            get => _rb.bodyType == RigidbodyType2D.Kinematic;
            set => _rb.bodyType = value ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }

        private Vector2 LipVelocity
        {
            get => _rb.linearVelocity;
            set => _rb.linearVelocity = value;
        }

        public Vector2 LipPosition
        {
            get => _rb.position;
            private set => _rb.position = value;
        }

        public float LipRotation
        {
            get => _rb.rotation;
            private set => _rb.rotation = value;
        }

        private void Start() => FollowBody();

        private void FixedUpdate()
        {
            if (0f < _attractCoolTimeCount) _attractCoolTimeCount -= Time.fixedDeltaTime;

            switch (CurrentState)
            {
                case PlayerLipState.FollowBody:
                    FollowBodyUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Attracted:
                    AttractedUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Attaching:
                    AttachingUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Dead:
                    FollowBodyUpdate(Time.fixedDeltaTime);
                    break;
            }
        }

        public void OnLipHitCollider(Collider2D col)
        {
            if (col == _ignoreCol) return;
            if (CurrentState != PlayerLipState.Attracted) return;

            if (col.TryGetComponent(out ILip otherLip))
            {
                if (!otherLip.IsKissableNow) return;

                Instantiate(_prefabKissConnector).Kiss(this, otherLip);
            }
            else if (col.TryGetComponent(out ILipAttachTarget attachTarget))
            {
                OnAttach(attachTarget);
            }
        }

        public void OnDamaged(int damageAmount, float nockbackPower, DamageType type)
        {
            /*
            switch (type)
            {
                case DamageType.Needle:
                    _sr.color = Color.magenta;
                    break;
            }*/
            _sr.color = Color.red;

            _bodyHealth.OnDamaged(damageAmount, (_bodyMove.Position - LipPosition).normalized * nockbackPower);
        }

        public bool TryRecover(int recoverAmount)
        {
            _sr.color = Color.white;
            return _bodyHealth.TryRecovered(recoverAmount);
        }

        public void OnDead()
        {
            CurrentState = PlayerLipState.Dead;
            LipVelocity = Vector2.zero;
            LipKinematic = true;
            LipPosition = LipPositionDefault;
            LipRotation = _bodyMove.Rotation;
            SetTarget(null);
            SetAttracterEnable(false);
        }

        private void SetAttracterEnable(bool value) => _objLipAttracter.SetActive(value);
        private void SetAttractCoolTime() => _attractCoolTimeCount = _attractedCoolTime;
        private void SetTarget(ILipAttachTarget target) => _target = target;

        private void FollowBody()
        {
            CurrentState = PlayerLipState.FollowBody;
            LipVelocity = Vector2.zero;
            LipKinematic = true;
            LipPosition = LipPositionDefault;
            LipRotation = _bodyMove.Rotation;
        }

        private void FollowBodyUpdate(float dt)
        {
            LipPosition = LipPositionDefault;
            LipRotation = _bodyMove.Rotation;
        }

        public void OnAttracted(Vector2 force)
        {
            if (CurrentState == PlayerLipState.Dead) return;
            if (CurrentState == PlayerLipState.Attaching) return;
            if (0f < _attractCoolTimeCount) return;

            if (_attractableRangeThreshoud < Vector2.Dot(OriginalCalculateUtils.AngleToDirection(_rb.rotation), force.normalized))
            {
                _attractedCancelTimeCounter = 0f;
                CurrentState = PlayerLipState.Attracted;

                LipKinematic = false;
                _rb.AddForce(force);

                if (LipVelocity.sqrMagnitude != 0f)
                {
                    LipRotation = OriginalCalculateUtils.DirectionToAngle(_rb.linearVelocity);
                }
            }
        }

        private void AttractedUpdate(float dt)
        {
            _attractedCancelTimeCounter += dt;

            if (_attractedCancelTime <= _attractedCancelTimeCounter)
            {
                _attractedCancelTimeCounter = 0f;
                CancelAttracted();
            }
        }

        private void CancelAttracted()
        {
            SetAttractCoolTime();
            FollowBody();
        }

        public void OnAttach(ILipAttachTarget target)
        {
            if (!MagnetJudgement.IsAttachable(_selfMagneticType, target.MagneticType)) return;

            CurrentState = PlayerLipState.Attaching;

            SetTarget(target);
            SetAttracterEnable(false);

            LipVelocity = Vector2.zero;
            LipKinematic = true;

            LipRotation = _target.GetAttachRotation(LipPosition);
            LipPosition = _target.GetAttachPoint(LipPosition);

            _attachedRotation = _target.GetInverseTransformRotation(LipRotation);
            _attachedPoint = _target.GetInverseTransformPoint(LipPosition);

            _target.OnAttached(this);
        }

        private void AttachingUpdate(float dt)
        {
            if (_target == null)
            {
                OnDetach();
            }
            else
            {
                LipPosition = _target.GetTransformPoint(_attachedPoint);
                LipRotation = _target.GetTransformRotation(_attachedRotation);

                Vector2 deltaVectorBodyToLip = LipPosition - _bodyMove.Position;
                float deltaSqrMagBodyToLip = deltaVectorBodyToLip.sqrMagnitude;

                if (_lipLengthMax * _lipLengthMax < deltaSqrMagBodyToLip)
                {
                    Vector2 pullForce =
                        deltaVectorBodyToLip
                        * Mathf.Clamp((deltaSqrMagBodyToLip - _lipLengthMax * _lipLengthMax) * _pullBodyPowerCoef, 0f, _pullBodyPowerMax);

                    _target.AddForce(-pullForce);
                    _bodyMove.AddForce(pullForce);
                }

                float angleBodyToLip = OriginalCalculateUtils.DirectionToAngle((LipPosition - _bodyMove.Position).normalized);
                float deltaAngleLipToBody = Mathf.DeltaAngle(LipRotation, angleBodyToLip);
                float allowAngle = 90f;

                if (allowAngle < Mathf.Abs(deltaAngleLipToBody))
                {
                    _target.AddTorque((Mathf.Abs(deltaAngleLipToBody) - allowAngle) * Mathf.Sign(deltaAngleLipToBody));
                }
            }
        }

        public void AddForceImpulseToAttachingTarget(Vector2 force)
        {
            if (_target != null) _target.AddForceImpulse(force);
        }

        public void OnDetach()
        {
            if (CurrentState != PlayerLipState.Attaching) return;

            if (_target != null) _target.OnDetached(this);

            SetTarget(null);
            SetAttracterEnable(true);
            SetAttractCoolTime();
            FollowBody();
        }

        public void OnAttachFromOther(ILipAttachTarget target, Vector2 inversePos, float inverseRot)
        {
            CurrentState = PlayerLipState.Attaching;

            SetTarget(target);
            SetAttracterEnable(false);

            LipVelocity = Vector2.zero;
            LipKinematic = true;

            _attachedPoint = inversePos;
            _attachedRotation = inverseRot;

            LipPosition = _target.GetTransformPoint(_attachedPoint);
            LipRotation = _target.GetTransformRotation(_attachedRotation);
        }

        public void OnDetachFromOther()
        {
            if (CurrentState != PlayerLipState.Attaching) return;

            SetTarget(null);
            SetAttracterEnable(true);
            SetAttractCoolTime();
            FollowBody();
        }
    }
}