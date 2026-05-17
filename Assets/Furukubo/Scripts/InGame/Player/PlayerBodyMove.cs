using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo(Refactoring of Mock_BodyMove)
    /// </summary>
    public class PlayerBodyMove : MonoBehaviour, IBlowTarget
    {
        [Header("Parameters")]
        [SerializeField] private float _initLookingAngle;
        [SerializeField] private float _dashPower;
        [SerializeField] private float _distanceFromLipMax;
        [SerializeField] private float _moveInputThreshoud;
        [SerializeField] private float _rotateSpeed;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerLip _lip;

        private Vector2 _moveInput;
        private bool _isIgnoreInput;

        public Vector2 Position => _rb.position;
        public float Rotation => _rb.rotation;

        private void Start()
        {
            _rb.rotation = _initLookingAngle;
        }

        private void FixedUpdate()
        {
            if (_moveInput.sqrMagnitude > _moveInputThreshoud * _moveInputThreshoud)
            {
                AddRotation(_moveInput, Time.fixedDeltaTime);
            }

            if (_lip == null) return;

            if (_lip.IsAttached)
            {
                Vector2 lipPos = _lip.transform.position;
                Vector2 between = _rb.position - lipPos;

                if (_distanceFromLipMax * _distanceFromLipMax < between.sqrMagnitude)
                {
                    Vector2 direction = between.normalized;

                    _rb.MovePosition(lipPos + direction * _distanceFromLipMax);

                    Vector2 outwardVel = Vector3.Project(_rb.linearVelocity, direction);
                    _rb.linearVelocity -= outwardVel;
                }
            }
        }

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
            Vector2 dir = CalculateUtilities.AngleToDirection(_rb.rotation);
            _rb.AddForce(dir * _dashPower, ForceMode2D.Impulse);
        }

        public void OnDetach()
        {
            if (_isIgnoreInput) return;
            if (_lip == null) return;
            _lip.OnDetachTarget();
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force);

        public void OnDead() => _isIgnoreInput = true;

        private void AddRotation(Vector2 targetDir, float deltaTime)
        {
            if (_rb == null) return;

            float currentAng = _rb.rotation;
            float targetAng = CalculateUtilities.DirectionToAngle(targetDir);
            float betweenAng = Mathf.DeltaAngle(currentAng, targetAng);

            float newAng = currentAng + betweenAng * _rotateSpeed * deltaTime;

            _rb.SetRotation(newAng);
        }
    }
}
