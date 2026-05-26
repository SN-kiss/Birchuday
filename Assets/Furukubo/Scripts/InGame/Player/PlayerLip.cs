using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Rigidbody2D _lipRb;
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private Transform _lipDefaultPointTr;
        [SerializeField] private Collider2D _ignoreCol;
        [SerializeField] private GameObject _objLipAttracter;
        [SerializeField] private KissConnector _prefabKissConnector;
        [SerializeField] private UnityEvent<int, Vector2> _onDamaged;

        private ILipAttachTarget _target;
        private PlayerLipState _currentState;
        private float _attachedRotationOffset;
        private Vector2 _attachedPositionOffset;
        private float _attractedCancelTimeCounter;
        private bool _isBodyDead;
        private float _attractCoolTimeCount;

        public Vector2 Position => _lipRb.position;
        public MagneticType MagneticType => _selfMagneticType;
        public bool IsAttached => _currentState == PlayerLipState.Attaching;
        public float Rotation => _lipRb.rotation;
        public bool IsKissableNow => _currentState != PlayerLipState.Attaching;
        public float LipLengthMax => _lipLengthMax;

        private void Start() => OnFollowBody();

        private void FixedUpdate()
        {
            if (0f < _attractCoolTimeCount) _attractCoolTimeCount -= Time.fixedDeltaTime;

            switch (_currentState)
            {
                case PlayerLipState.FollowBody:
                    FollowBodyStateUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Attracted:
                    AttractedStateUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Attaching:
                    AttachingOnTargetStateUpdate(Time.fixedDeltaTime);
                    break;
            }
        }

        public void OnLipHitCollider(Collider2D col)
        {
            if (col == _ignoreCol) return;
            if (_currentState != PlayerLipState.Attracted) return;

            if (col.TryGetComponent(out ILip otherLip))
            {
                if (!otherLip.IsKissableNow) return;

                Instantiate(_prefabKissConnector).Kiss(this, otherLip);
            }
            else if (col.TryGetComponent(out ILipAttachTarget attachTarget))
            {
                OnNormalAttach(attachTarget);
            }
        }

        private void SetAttractCoolTime() => _attractCoolTimeCount = _attractedCoolTime;

        private void OnFollowBody()
        {
            _currentState = PlayerLipState.FollowBody;
            _lipRb.linearVelocity = Vector2.zero;
            _lipRb.bodyType = RigidbodyType2D.Kinematic;
            _lipRb.position = _lipDefaultPointTr.position;
            _lipRb.rotation = _bodyRb.rotation;
        }

        private void FollowBodyStateUpdate(float dt)
        {
            _lipRb.position = _lipDefaultPointTr.position;
            _lipRb.rotation = _bodyRb.rotation;
        }

        public void OnAttracted(Vector2 force)
        {
            if (_isBodyDead) return;
            if (_currentState == PlayerLipState.Attaching) return;
            if (0f < _attractCoolTimeCount) return;

            if (_attractableRangeThreshoud < Vector2.Dot(CalculateUtilities.AngleToDirection(_lipRb.rotation), force.normalized))
            {
                _attractedCancelTimeCounter = 0f;
                _currentState = PlayerLipState.Attracted;
                _lipRb.bodyType = RigidbodyType2D.Dynamic;

                _lipRb.AddForce(force);

                if (_lipRb.linearVelocity.sqrMagnitude != 0f)
                    _lipRb.rotation = CalculateUtilities.DirectionToAngle(_lipRb.linearVelocity);
            }
        }

        private void AttractedStateUpdate(float dt)
        {
            _attractedCancelTimeCounter += dt;

            if (_attractedCancelTime <= _attractedCancelTimeCounter)
            {
                _attractedCancelTimeCounter = 0f;
                OnCancelAttracted();
            }
        }

        private void OnCancelAttracted()
        {
            SetAttractCoolTime();

            OnFollowBody();
        }

        public void OnNormalAttach(ILipAttachTarget target)
        {
            if (!MagnetJudgement.IsAttachable(_selfMagneticType, target.MagneticType)) return;

            _currentState = PlayerLipState.Attaching;

            _objLipAttracter.SetActive(false);

            _target = target;

            _lipRb.linearVelocity = Vector2.zero;
            _lipRb.bodyType = RigidbodyType2D.Kinematic;

            _lipRb.rotation = _target.GetAttachRotation(_lipRb.position);
            _lipRb.position = _target.GetAttachPoint(_lipRb.position);

            _attachedRotationOffset = _target.GetInverseTransformRotation(_lipRb.rotation);//angle degree
            _attachedPositionOffset = _target.GetInverseTransformPoint(_lipRb.position);

            target.OnAttached(this);
        }

        private void AttachingOnTargetStateUpdate(float dt)
        {
            if (_target == null)
            {
                OnNormalDetach();
            }
            else
            {
                _lipRb.position = _target.GetTransformPoint(_attachedPositionOffset);

                Vector2 deltaVectorBodyToLip = _lipRb.position - _bodyRb.position;
                float deltaSqrMagBodyToLip = deltaVectorBodyToLip.sqrMagnitude;

                if (_lipLengthMax * _lipLengthMax < deltaSqrMagBodyToLip)
                {
                    Vector2 pullForce =
                        deltaVectorBodyToLip
                        * Mathf.Clamp((deltaSqrMagBodyToLip - _lipLengthMax * _lipLengthMax) * _pullBodyPowerCoef, 0f, _pullBodyPowerMax);

                    _target.AddForce(-pullForce);
                    _bodyRb.AddForce(pullForce);
                }

                _lipRb.rotation = _target.GetTransformRotation(_attachedRotationOffset);

                //target rotation
                float angleBodyToLip = CalculateUtilities.DirectionToAngle((_lipRb.position - _bodyRb.position).normalized);
                float deltaAngleLipToBody = Mathf.DeltaAngle(_lipRb.rotation, angleBodyToLip);

                float allowAngle = 90f;

                if (allowAngle < Mathf.Abs(deltaAngleLipToBody))
                {
                    _target.AddTorque((Mathf.Abs(deltaAngleLipToBody) - allowAngle) * Mathf.Sign(deltaAngleLipToBody));
                }

                /*
                //body rotation
                float deltaAngleBodyToLip = Mathf.DeltaAngle(_bodyRb.rotation, angleBodyToLip);

                if (allowAngle < Mathf.Abs(deltaAngleBodyToLip))
                {
                    _bodyRb.AddTorque((Mathf.Abs(deltaAngleBodyToLip) - allowAngle) * Mathf.Sign(deltaAngleBodyToLip));
                }*/
            }
        }

        public void AddForceImpulseToAttachingTarget(Vector2 force)
        {
            if (_target == null) return;
            _target.AddImpulse(force);
        }

        public void OnNormalDetach()
        {
            if (_currentState != PlayerLipState.Attaching) return;

            _objLipAttracter.SetActive(true);

            if (_target != null)
            {
                _target.OnDetached(this);
            }

            _target = null;

            SetAttractCoolTime();

            OnFollowBody();
        }

        public void OnLipDamaged(int damageAmount, float nockbackPower, DamageType type)
        {
            Vector2 nockback = (_bodyRb.position - _lipRb.position).normalized * nockbackPower;
            _onDamaged?.Invoke(damageAmount, nockback);

            switch (type)
            {
                case DamageType.None:
                    _sr.color = _selfMagneticType == MagneticType.North ? Color.blue : Color.red;
                    break;
                case DamageType.Needle:
                    _sr.color = Color.magenta;
                    break;
                case DamageType.Heat:
                    _sr.color = Color.orange;
                    break;
            }
        }

        public void OnBodyDamaged()
        {
            if (_currentState == PlayerLipState.Attaching)
            {
                OnNormalDetach();
            }
            else if (_currentState == PlayerLipState.Attracted)
            {
                OnCancelAttracted();
            }
        }

        public void OnBodyDead() => _isBodyDead = true;

        public void OnKissAttach(ILipAttachTarget target, Vector2 inversePos, float inverseRot)
        {
            _currentState = PlayerLipState.Attaching;

            _target = target;

            _objLipAttracter.SetActive(false);

            _lipRb.linearVelocity = Vector2.zero;
            _lipRb.bodyType = RigidbodyType2D.Kinematic;

            _attachedPositionOffset = inversePos;
            _attachedRotationOffset = inverseRot;

            _lipRb.position = _target.GetTransformPoint(inversePos);
            _lipRb.rotation = _target.GetTransformRotation(inverseRot);
        }

        public void OnKissDetach()
        {
            if (_currentState != PlayerLipState.Attaching) return;

            _target = null;

            _objLipAttracter.SetActive(true);

            SetAttractCoolTime();

            OnFollowBody();
        }

        private enum PlayerLipState
        {
            FollowBody,
            Attracted,
            Attaching,
            //KissAttaching
        }
    }
}