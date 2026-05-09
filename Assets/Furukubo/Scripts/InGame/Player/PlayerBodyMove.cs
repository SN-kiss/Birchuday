using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo(Fixed BodyMove_GOD)
    /// </summary>
    public class PlayerBodyMove : MonoBehaviour
    {
        [Header("Initialize Parameters")]
        [SerializeField] private float _initLookingAngle;

        [Header("Parameters")]
        [SerializeField] private float _dashPower;
        [SerializeField] private float _gravityScale;
        [SerializeField] private float _linearDamping;
        [SerializeField] private float _distanceFromLipMax;
        [SerializeField] private float _moveInputThreshoud;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        //[SerializeField] private LipController _lip;

        private Vector2 _moveInput;
        private Vector2 _lookingDirection;
        private bool _isIgnoreInput;

        private void Start()
        {
            if (_rb != null)
            {
                _rb.gravityScale = _gravityScale;
                _rb.linearDamping = _linearDamping;
            }

            SetLookingDirection(_initLookingAngle);
        }

        private void Update()
        {
            if (_isIgnoreInput) _moveInput = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if (_moveInput.sqrMagnitude > _moveInputThreshoud * _moveInputThreshoud)
            {
                SetLookingDirection(_moveInput);
            }

            /*
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
            }*/
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                transform.localEulerAngles = new Vector3(0f, 0f, _initLookingAngle);
            }
#endif
        }

        //Sended message from Input Action
        public void OnMove(InputValue value)
        {
            if (_isIgnoreInput) return;
            _moveInput = value.Get<Vector2>();
        }

        //Sended message from Input Action
        public void OnDash()
        {
            if (_isIgnoreInput) return;
            if (_rb == null) return;
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(_lookingDirection * _dashPower, ForceMode2D.Impulse);
        }

        //Sended message from Input Action ?
        public void OnDetach()
        {
            /*
            if (_isIgnoreInput) return;
            if (_lip == null) return;
            _lip.Detach();
            */
        }

        public void SetIgnoreInput(bool value)
        {
            _isIgnoreInput = value;
        }
        
        private void SetLookingDirection(float angle)
        {
            if (_rb == null) return;

            _lookingDirection = AngleToDirection(angle);
            _rb.SetRotation(angle);
        }

        private void SetLookingDirection(Vector2 direction)
        {
            if (_rb == null) return;

            _lookingDirection = direction;
            _rb.SetRotation(DirectionToAngle(direction));
        }

        private float DirectionToAngle(Vector2 dir) => Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        private Vector2 AngleToDirection(float deg)
        {
            float radiun = deg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radiun), Mathf.Sin(radiun));
        }
    }
}
