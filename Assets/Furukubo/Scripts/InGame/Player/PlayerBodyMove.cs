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
        [SerializeField] private float _moveInputThreshoud;
        [SerializeField] private float _rotateSpeed;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerLip _lip;

        private Vector2 _rotateInput;
        private bool _isIgnoreInput;

        public Vector2 Position => _rb.position;
        public float Rotation => _rb.rotation;

        private void Start()
        {
            _rb.rotation = _initLookingAngle;
        }

        private void FixedUpdate()
        {
            if (_rotateInput.sqrMagnitude > _moveInputThreshoud * _moveInputThreshoud)
            {
                AddRotation(_rotateInput, Time.fixedDeltaTime);
            }
        }

        public void OnRotate(InputValue value)
        {
            if (_isIgnoreInput) return;

            _rotateInput = value.Get<Vector2>();
        }

        public void OnDash()
        {
            if (_isIgnoreInput) return;
            if (_rb == null) return;
            if(_lip == null) return;

            AddImpulse(CalculateUtilities.AngleToDirection(_rb.rotation) * _dashPower);
        }

        public void OnDetach()
        {
            if (_isIgnoreInput) return;
            if (_lip == null) return;
            _lip.OnNormalDetach();
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force);

        private void AddImpulse(Vector2 force)
        {
            if (_lip.IsAttached)
            {
                Vector2 between = _rb.position - _lip.Position;
                float sqrMag = between.sqrMagnitude;

                float lipLengthMax = _lip.LipLengthMax;

                if (lipLengthMax * lipLengthMax < sqrMag)
                {
                    _rb.linearVelocity = Vector2.zero;
                    _rb.AddForce(force, ForceMode2D.Impulse);

                    float dot = Mathf.Clamp01(Vector2.Dot(force.normalized, between.normalized));
                    _lip.AddImpulseToAttachingTarget(between.normalized * dot * _dashPower);
                }
                else
                {
                    _rb.linearVelocity = Vector2.zero;
                    _rb.AddForce(force, ForceMode2D.Impulse);
                }
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
                _rb.AddForce(force, ForceMode2D.Impulse);
            }
        }

        public void OnDead() => _isIgnoreInput = true;

        private void AddRotation(Vector2 targetDir, float deltaTime)
        {
            if (_rb == null) return;

            float currentAng = _rb.rotation;
            float targetAng = CalculateUtilities.DirectionToAngle(targetDir);
            float betweenAng = Mathf.DeltaAngle(currentAng, targetAng);

            float newAng = currentAng + betweenAng * _rotateSpeed * deltaTime;

            _rb.SetRotation(newAng);
            _rb.angularVelocity = 0f;
        }
    }
}
