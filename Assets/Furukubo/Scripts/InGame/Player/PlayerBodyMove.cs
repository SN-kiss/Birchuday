using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo(Refactoring of Mock_BodyMove)
    /// </summary>
    public class PlayerBodyMove : MonoBehaviour, IBlowTarget, IBlackHoleTarget
    {
        [Header("Parameters")]
        [SerializeField] private float _initLookingAngle;
        [SerializeField] private float _dashPower;
        [SerializeField] private float _moveInputThreshoud;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _playDashSEIntervalTime;

        [Header("References")]
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private PlayerLip _lip;
        [SerializeField] private AudioSource _dashAudioSource;
        [SerializeField] private AudioClip _dashAudioClip;
        [SerializeField] private UnityEvent _onDashEvent;

        public event Action<Vector2> _onBlackHoleDead;

        private Vector2 _rotateInput;
        private bool _isIgnoreInput;

        public Vector2 Velocity => _bodyRb.linearVelocity;
        public Vector2 Position => _bodyRb.position;
        public float Rotation => _bodyRb.rotation;

        private void Start()
        {
            _bodyRb.rotation = _initLookingAngle;
        }

        private void FixedUpdate()
        {
            if (_rotateInput.sqrMagnitude > _moveInputThreshoud * _moveInputThreshoud)
            {
                AddRotation(_rotateInput, Time.fixedDeltaTime);
            }
        }

        public void OnMove(InputValue value)
        {
            if (_isIgnoreInput) return;

            _rotateInput = value.Get<Vector2>();
        }

        public void OnDash()
        {
            if (_isIgnoreInput) return;
            if (_bodyRb == null) return;
            if(_lip == null) return;

            _onDashEvent?.Invoke();

            AddForceImpulse(OriginalCalculateUtils.AngleToDirection(_bodyRb.rotation) * _dashPower);

            if (_dashAudioSource == null || _dashAudioClip == null) return;
            if (!_dashAudioSource.isPlaying) _dashAudioSource.PlayOneShot(_dashAudioClip);
        }

        public void OnDetach()
        {
            if (_isIgnoreInput) return;
            if (_lip == null) return;
            _lip.OnDetach();
        }

        public void SetIgnoreInput(bool value)
        {
            _isIgnoreInput = value;
        }

        public void AddForce(Vector2 force) => _bodyRb.AddForce(force);

        public void AddForceImpulse(Vector2 force)
        {
            _bodyRb.linearVelocity = Vector2.zero;
            _bodyRb.AddForce(force, ForceMode2D.Impulse);

            if (_lip.CurrentState == PlayerLipState.Attaching)
            {
                Vector2 between = _bodyRb.position - _lip.LipPosition;
                float sqrMag = between.sqrMagnitude;

                float lipLengthMax = _lip.LipLengthMax;

                if (lipLengthMax * lipLengthMax < sqrMag)
                {
                    float dot = Mathf.Clamp01(Vector2.Dot(force.normalized, between.normalized));
                    _lip.AddForceImpulseToAttachingTarget(between.normalized * dot * _dashPower * 0.75f);
                }
            }
        }

        public void AddTorque(float torque) => _bodyRb.AddTorque(torque);

        private void AddRotation(Vector2 targetDir, float deltaTime)
        {
            if (_bodyRb == null) return;

            float currentAng = _bodyRb.rotation;
            float targetAng = OriginalCalculateUtils.DirectionToAngle(targetDir);
            float betweenAng = Mathf.DeltaAngle(currentAng, targetAng);

            float newAng = currentAng + betweenAng * _rotateSpeed * deltaTime;

            _bodyRb.SetRotation(newAng);
            _bodyRb.angularVelocity = 0f;
        }

        public void OnHitBlackhole(Vector2 blackHolePos)
        {
            _onBlackHoleDead?.Invoke(blackHolePos);
        }
    }
}
