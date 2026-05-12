using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo(Refactoring of Mock_BodyMove)
    /// </summary>
    public class PlayerBodyMove : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _initLookingAngle;
        [SerializeField] private float _dashPower;
        [SerializeField] private float _distanceFromLipMax;
        [SerializeField] private float _moveInputThreshoud;
        [SerializeField] private float _rotateSpeed;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerLipControler _lip;

        private Vector2 _moveInput;
        private Vector2 _lookingDirection;
        private bool _isIgnoreInput;

        public Vector2 Position => _rb.position;

        private void Start()
        {
            _lookingDirection = CalculateUtilities.AngleToDirection(_initLookingAngle);
            _rb.rotation = _initLookingAngle;
        }

        private void Update()
        {
            if (_isIgnoreInput) _moveInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (_moveInput.sqrMagnitude > _moveInputThreshoud * _moveInputThreshoud)
            {
                UpdateRotation(_moveInput, Time.fixedDeltaTime);
            }

            if (_lip == null) return;

            if (_lip.IsAttached)
            {
                Vector2 lipPos = _lip.transform.position;
                Vector2 between = _rb.position - lipPos;

                if (between.sqrMagnitude > _distanceFromLipMax * _distanceFromLipMax)
                {
                    Vector2 direction = between.normalized;

                    _rb.MovePosition(lipPos + direction * _distanceFromLipMax);

                    Vector2 outwardVel = Vector3.Project(_rb.linearVelocity, direction);
                    _rb.linearVelocity -= outwardVel;
                }
            }
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force);

        public void OnMove(InputValue value)
        {
            if (_isIgnoreInput) return;
            _moveInput = value.Get<Vector2>();
        }

        public void OnDash()
        {
            if (_isIgnoreInput) return;
            if (_rb == null) return;
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(_lookingDirection * _dashPower, ForceMode2D.Impulse);
        }

        public void OnDetach()
        {
            if (_isIgnoreInput) return;
            if (_lip == null) return;
            _lip.Detach();
        }

        public void SetIgnoreInput(bool value)
        {
            _isIgnoreInput = value;
        }
        
        private void UpdateRotation(Vector2 targetDir, float deltaTime)
        {
            if (_rb == null) return;

            float currentAng = _rb.rotation;
            float targetAng = CalculateUtilities.DirectionToAngle(targetDir);
            float betweenAng = Mathf.DeltaAngle(currentAng, targetAng);

            float newAng = currentAng + betweenAng * _rotateSpeed * deltaTime;

            _lookingDirection = CalculateUtilities.AngleToDirection(newAng);
            _rb.SetRotation(newAng);
        }
    }
}
