using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of LipControler)
    /// </summary>
    public class PlayerLip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerBodyMove _body;
        [SerializeField] private Transform _lipDefaultPointTr;

        [Header("Parameters")]
        [SerializeField] private float _attractableAngle;
        [SerializeField] private float _attachDistance;
        [SerializeField] private float _pullBodyPower;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _pullStopDistance;

        private LipConnecter _target;
        private PlayerLipState _currentState;
        private Vector2 _attachedLocalOffset;

        public bool IsAttached => _currentState == PlayerLipState.FollowingTarget;
        public Vector2 Position => _rb.position;

        private float _attractedCancelCounter;

        private void Start()
        {
            OnStartFollowingBodyState();
        }

        private void FixedUpdate()
        {
            switch (_currentState)
            {
                case PlayerLipState.FollowingBody:
                    FollowingBodyStateUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.Attracted:
                    AttractedStateUpdate(Time.fixedDeltaTime);
                    break;
                case PlayerLipState.FollowingTarget:
                    FollowingTargetStateUpdate(Time.fixedDeltaTime);
                    break;
            }
        }

        private void OnStartFollowingBodyState()
        {
            _currentState = PlayerLipState.FollowingBody;
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.position = _lipDefaultPointTr.position;
            _rb.rotation = _lipDefaultPointTr.localEulerAngles.z;
        }

        private void FollowingBodyStateUpdate(float dt)
        {
            _rb.position = _lipDefaultPointTr.position;
            _rb.rotation = _body.Rotation;
        }

        public void OnAttracted(Vector2 force, Vector2 closestPoint, LipConnecter connecter)
        {
            if (_currentState == PlayerLipState.FollowingTarget) return;

            float currentAng = _rb.rotation;
            float forceAngle = CalculateUtilities.DirectionToAngle(force);

            if (Mathf.Abs(Mathf.DeltaAngle(currentAng, forceAngle)) <= _attractableAngle)
            {
                _attractedCancelCounter = 0f;
                _currentState = PlayerLipState.Attracted;
                _rb.bodyType = RigidbodyType2D.Dynamic;

                _rb.AddForce(force);

                if (_rb.linearVelocity.sqrMagnitude != 0f)
                    _rb.rotation = CalculateUtilities.DirectionToAngle(_rb.linearVelocity);

                if ((_rb.position - closestPoint).sqrMagnitude < _attachDistance * _attachDistance)
                {
                    _currentState = PlayerLipState.FollowingTarget;
                    _target = connecter;
                    _rb.linearVelocity = Vector2.zero;
                    _rb.bodyType = RigidbodyType2D.Kinematic;
                    _rb.position = closestPoint;
                    _attachedLocalOffset = _target.GetInverseTransformPoint(_rb.position);
                }
            }
        }

        private void AttractedStateUpdate(float dt)
        {
            _attractedCancelCounter += dt;

            if (1f <= _attractedCancelCounter)
            {
                _attractedCancelCounter = 0f;
                OnCancelAttracted();
            }
        }

        private void OnCancelAttracted()
        {
            OnStartFollowingBodyState();
        }

        private void FollowingTargetStateUpdate(float dt)
        {
            if (_target == null)
            {
                OnStartFollowingBodyState();
            }
            else
            {
                _rb.position = _target.GetTransformPoint(_attachedLocalOffset);
                _rb.rotation = CalculateUtilities.DirectionToAngle(_target.Position - Position);

                Vector2 betweenToBody = _rb.position - _body.Position;
                float distanceToBody = betweenToBody.magnitude;

                if (_pullStopDistance < distanceToBody)
                {
                    Vector2 pullForce =
                        betweenToBody.normalized
                        * Mathf.Clamp((distanceToBody - _pullStopDistance) * _pullBodyPower, 0f, _pullBodyPowerMax);

                    _target.AddForce(-pullForce);
                    _body.AddForce(pullForce);
                }
            }
        }

        public void OnCancelFollowingTarget()
        {
            if (_currentState != PlayerLipState.FollowingTarget) return;
            _target = null;
            OnStartFollowingBodyState();
        }

        private enum PlayerLipState
        {
            FollowingBody,
            Attracted,
            FollowingTarget,
        }
    }
}