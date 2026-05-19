using UnityEngine;
using UnityEngine.Events;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of LipControler)
    /// </summary>
    public class PlayerLip : MonoBehaviour, ILip
    {
        [Header("Parameters")]
        [SerializeField, Range(-1f, 1f)] private float _attractableRangeThreshoud;
        [SerializeField] private float _attractedCancelTime;
        [SerializeField] private float _attachDistance;
        [SerializeField] private float _pullBodyPower;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _pullStopDistance;
        [SerializeField] private float _attractCoolTime;

        [Header("References")]
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private Transform _lipDefaultPointTr;
        [SerializeField] private Collider2D _ignoreCol;
        [SerializeField] private UnityEvent<int, Vector2> _onDamaged;

        private ILipAttachTarget _target;
        private PlayerLipState _currentState;
        private Vector2 _attachedLocalOffset;
        private float _attractedCancelTimeCounter;
        private bool _isBodyDead;
        private float _attractCoolTimeCount;

        public bool IsAttached => _currentState == PlayerLipState.AttachOnTarget;
        public Vector2 Position => _rb.position;

        private void Start()
        {
            OnFollowBody();
        }

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
                case PlayerLipState.AttachOnTarget:
                    AttachOnTargetStateUpdate(Time.fixedDeltaTime);
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col == _ignoreCol) return;
            if (_currentState != PlayerLipState.Attracted) return;

            if (col.TryGetComponent(out ILipAttachTarget target)) OnAttachOnTarget(target);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col == _ignoreCol) return;
            if (_currentState != PlayerLipState.Attracted) return;

            if (col.TryGetComponent(out ILipAttachTarget target)) OnAttachOnTarget(target);
        }

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
            if (_currentState == PlayerLipState.AttachOnTarget) return;
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
            StartAttractCoolTime();

            OnFollowBody();
        }

        private void OnAttachOnTarget(ILipAttachTarget target)
        {
            Vector2 closestPoint = target.GetClosestPoint(_rb.position);

            _currentState = PlayerLipState.AttachOnTarget;
            _target = target;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.position = closestPoint;
            _attachedLocalOffset = _target.GetInverseTransformPoint(_rb.position);

            target.OnAttached(this);
        }

        private void AttachOnTargetStateUpdate(float dt)
        {
            if (_target == null)
            {
                OnDetachTarget();
            }
            else
            {
                _rb.position = _target.GetTransformPoint(_attachedLocalOffset);
                _rb.rotation = CalculateUtilities.DirectionToAngle(_target.Position - Position);

                Vector2 betweenToBody = _rb.position - _bodyRb.position;
                float distanceToBody = betweenToBody.magnitude;

                if (_pullStopDistance < distanceToBody)
                {
                    Vector2 pullForce =
                        betweenToBody.normalized
                        * Mathf.Clamp((distanceToBody - _pullStopDistance) * _pullBodyPower, 0f, _pullBodyPowerMax);

                    _target.AddForce(-pullForce);
                    _bodyRb.AddForce(pullForce);
                }
            }
        }

        public void OnDetachTarget()
        {
            if (_currentState != PlayerLipState.AttachOnTarget) return;

            if (_target != null)
            {
                _target.OnDetached();
                _target = null;
            }

            StartAttractCoolTime();

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
            if (_currentState == PlayerLipState.AttachOnTarget)
            {
                OnDetachTarget();
            }
            else if (_currentState == PlayerLipState.Attracted)
            {
                OnCancelAttracted();
            }
        }

        public void OnBodyDead()
        {
            _isBodyDead = true;
        }

        private void StartAttractCoolTime() => _attractCoolTimeCount = _attractCoolTime;

        private enum PlayerLipState
        {
            FollowBody,
            Attracted,
            AttachOnTarget,
        }
    }
}