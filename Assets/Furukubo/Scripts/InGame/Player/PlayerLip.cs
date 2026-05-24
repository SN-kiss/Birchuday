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
        [SerializeField] private float _pullBodyPowerBase;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _lipLengthMax;

        [Header("References")]
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Rigidbody2D _rb;
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

        public bool IsAttached => _currentState == PlayerLipState.Attaching;
        public Vector2 Position => _rb.position;
        public float Rotation => _rb.rotation;

        public bool IsKissableNow => true;
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

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col == _ignoreCol) return;
            if (_currentState != PlayerLipState.Attracted) return;

            if (col.TryGetComponent(out ILip otherLip))
            {
                Instantiate(_prefabKissConnector).Kiss(this, otherLip);
            }
            else if (col.TryGetComponent(out ILipAttachTarget attachTarget))
            {
                OnNormalAttach(attachTarget);
            }
        }

        /*
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col == _ignoreCol) return;
            if (_currentState != PlayerLipState.Attracted) return;

            if (false)
            {
                //相手がキス対象だったらクッションを生成してクッション側でキス専用アタッチ
                //クッション生成
            }
            else if (col.TryGetComponent(out ILipAttachTarget target))
            {
                //それ以外の普通に接続する対象なら普通にアタッチ
                OnNormalAttach(target);
            }
        }
        */

        private void SetAttractCoolTime() => _attractCoolTimeCount = _attractedCoolTime;

        private void OnFollowBody()
        {
            _currentState = PlayerLipState.FollowBody;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.position = _lipDefaultPointTr.position;
            _rb.rotation = _bodyRb.rotation;
        }

        private void FollowBodyStateUpdate(float dt)
        {
            _rb.position = _lipDefaultPointTr.position;
            _rb.rotation = _bodyRb.rotation;
        }

        public void OnAttracted(Vector2 force)
        {
            if (_isBodyDead) return;
            if (_currentState == PlayerLipState.Attaching) return;
            if (0f < _attractCoolTimeCount) return;

            if (_attractableRangeThreshoud < Vector2.Dot(CalculateUtilities.AngleToDirection(_rb.rotation), force.normalized))
            {
                _attractedCancelTimeCounter = 0f;
                _currentState = PlayerLipState.Attracted;
                _rb.bodyType = RigidbodyType2D.Dynamic;

                _rb.AddForce(force);

                if (_rb.linearVelocity.sqrMagnitude != 0f)
                    _rb.rotation = CalculateUtilities.DirectionToAngle(_rb.linearVelocity);
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
            _currentState = PlayerLipState.Attaching;

            _objLipAttracter.SetActive(false);

            _target = target;

            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            _rb.position = _target.GetAttachPoint(_rb.position);
            _rb.rotation = _target.GetAttachRotation(_rb.position);

            _attachedPositionOffset = _target.GetInverseTransformPoint(_rb.position);
            _attachedRotationOffset = _target.GetInverseTransformRotation(_rb.rotation);//angle degree

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
                _rb.position = _target.GetTransformPoint(_attachedPositionOffset);
                _rb.rotation = _target.GetTransformRotation(_attachedRotationOffset);

                Vector2 betweenToBody = _rb.position - _bodyRb.position;
                float sqrDistanceToBody = betweenToBody.sqrMagnitude;

                if (_lipLengthMax * _lipLengthMax < sqrDistanceToBody)
                {
                    Vector2 pullForce =
                        betweenToBody
                        * Mathf.Clamp((sqrDistanceToBody - _lipLengthMax * _lipLengthMax) * _pullBodyPowerBase, 0f, _pullBodyPowerMax);

                    _target.AddForce(-pullForce);
                    _bodyRb.AddForce(pullForce);
                }
            }
        }

        public void AddImpulseToAttachingTarget(Vector2 force)
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

        public void OnLipDamage(int damageAmount, float nockbackPower, LipDamageType type)
        {
            Vector2 nockback = (_bodyRb.position - _rb.position).normalized * nockbackPower;
            _onDamaged?.Invoke(damageAmount, nockback);

            switch (type)
            {
                case LipDamageType.None:
                    _sr.color = Color.red;
                    break;
                case LipDamageType.Needle:
                    _sr.color = Color.magenta;
                    break;
                case LipDamageType.Heat:
                    _sr.color = Color.yellow;
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

            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;

            _attachedPositionOffset = inversePos;
            _attachedRotationOffset = inverseRot;

            _rb.position = _target.GetTransformPoint(inversePos);
            _rb.rotation = _target.GetTransformRotation(inverseRot);
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