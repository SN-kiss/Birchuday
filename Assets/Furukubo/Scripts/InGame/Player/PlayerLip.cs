using System;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of LipControler)
    /// </summary>
    public class PlayerLip : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _attractableAngle;
        [SerializeField] private float _attractedCancelTime;
        [SerializeField] private float _attachDistance;
        [SerializeField] private float _pullBodyPower;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _pullStopDistance;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private Transform _lipDefaultPointTr;

        private ILipAttacher _target;
        private PlayerLipState _currentState;
        private Vector2 _attachedLocalOffset;
        private float _attractedCancelTimeCounter;

        public bool IsAttached => _currentState == PlayerLipState.AttachOnTarget;
        public Vector2 Position => _rb.position;

        private void Start()
        {
            OnFollowBody();
        }

        private void FixedUpdate()
        {
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_currentState != PlayerLipState.Attracted) return;

            if (collision.TryGetComponent(out ILipAttacher target)) OnAttachOnTarget(target);
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
            if (_currentState == PlayerLipState.AttachOnTarget) return;

            float currentAng = _rb.rotation;
            float forceAngle = CalculateUtilities.DirectionToAngle(force);

            if (Mathf.Abs(Mathf.DeltaAngle(currentAng, forceAngle)) <= _attractableAngle)
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
            OnFollowBody();
        }

        private void OnAttachOnTarget(ILipAttacher target)
        {
            Vector2 closestPoint = target.GetClosestPoint(_rb.position);

            if ((_rb.position - closestPoint).sqrMagnitude < _attachDistance * _attachDistance)
            {
                _currentState = PlayerLipState.AttachOnTarget;
                _target = target;
                _rb.linearVelocity = Vector2.zero;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _rb.position = closestPoint;
                _attachedLocalOffset = _target.GetInverseTransformPoint(_rb.position);
            }

            target.OnAttached();
        }

        private void AttachOnTargetStateUpdate(float dt)
        {
            if (_target == null)
            {
                OnCancelAttachOnTarget();
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

        public void OnCancelAttachOnTarget()
        {
            if (_currentState != PlayerLipState.AttachOnTarget) return;

            if (_target != null)
            {
                _target.OnDetached();
                _target = null;
            }

            OnFollowBody();
        }

        private enum PlayerLipState
        {
            FollowBody,
            Attracted,
            AttachOnTarget,
        }
    }
}