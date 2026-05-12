using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of LipControler)
    /// </summary>
    public class PlayerLipControler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerBodyMove _body;
        [SerializeField] private Transform _playerMainTr;
        [SerializeField] private Transform _lipDefaultPointTr;

        [Header("Parameters")]
        [SerializeField] private float _attractSpeed;
        [SerializeField] private float _attachDistance;
        [SerializeField] private float _pullBodyPower;
        [SerializeField] private float _pullBodyPowerMax;
        [SerializeField] private float _pullStopDistance;

        private ILipConnecter _target;
        private PlayerLipState _currentState;
        private Vector2 _attachedLocalOffset;

        public bool IsAttached => _currentState == PlayerLipState.FollowTarget;

        private void Start()
        {
            FollowBodyStateEnter();
        }

        private void FixedUpdate()
        {
            switch (_currentState)
            {
                case PlayerLipState.Attracted:
                    AttractedStateUpdate();
                    break;
                case PlayerLipState.FollowTarget:
                    FollowTargetStateUpdate();
                    break;
            }
        }

        public void Detach()
        {
            _target = null;
            FollowBodyStateEnter();
        }

        private void FollowBodyStateEnter()
        {
            _currentState = PlayerLipState.FollowBody;

            if(_body.transform != null) transform.SetParent(_body.transform);
            transform.localPosition = _lipDefaultPointTr.localPosition;
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);

            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        public void AttractedStateEnter(ILipConnecter target)
        {
            if (_currentState != PlayerLipState.FollowBody) return;
            transform.SetParent(_playerMainTr);
            _target = target;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _currentState = PlayerLipState.Attracted;
        }

        private void AttractedStateUpdate()
        {
            if (_target == null) 
            {
                FollowBodyStateEnter();
                return;
            }

            Vector2 closestPoint = _target.GetClosestPoint(_rb.position);
            Vector2 betweenToClosestPoint = closestPoint - _rb.position;
            
            _rb.linearVelocity = betweenToClosestPoint.normalized * _attractSpeed;

            if(_rb.linearVelocity.sqrMagnitude != 0f) _rb.rotation = CalculateUtilities.DirectionToAngle(_rb.linearVelocity);

            if (betweenToClosestPoint.sqrMagnitude < _attachDistance * _attachDistance)
            {
                //Attach to target
                _rb.linearVelocity = Vector2.zero;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _attachedLocalOffset = _target.GetInverseTransformPoint(_rb.position);
                _currentState = PlayerLipState.FollowTarget;
            }
        }

        private void FollowTargetStateUpdate()
        {
            if (_target == null)
            {
                FollowBodyStateEnter();
                return;
            }

            _rb.position = _target.GetTransformPoint(_attachedLocalOffset);
            _rb.rotation = _target.Rotation;

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

        private enum PlayerLipState
        {
            FollowBody,
            Attracted,
            FollowTarget,
        }
    }
}