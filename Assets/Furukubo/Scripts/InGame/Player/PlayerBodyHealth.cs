using InGame.Effect;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerBodyHealth : MonoBehaviour, IDamageTarget
    {
        [Header("Parameters")]
        [SerializeField] private int _healthMax;
        [SerializeField] private float _invincibleTime;
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private AudioClip _damagedAudioClip;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerBodyMove _bodyMove;
        [SerializeField] private PlayerLip _lip;
        [SerializeField] private EffectGenerator _damagedEffect;
        [SerializeField] private PlayerSpriteChange _spriteChange;
        [SerializeField] private ParticleSystem _psRecovery;
        [SerializeField] private DeviceVibrationData _deviceVibrationData;

        public event Action OnDamagedEvent;
        public event Action OnDeadEvent;
        public event Action<int, int> OnHealthChangedEvent;
        private int _remainHealth;
        private float _remainInvincibleTime;

        public MagneticType MagneticType => _selfMagneticType;
        public Vector2 Position => _rb.position;
        private bool IsInvincible => 0f < _remainInvincibleTime;
        private bool IsDead => _remainHealth <= 0;

        private bool _ignoreDamage;

        private void Start()
        {
            _remainHealth = _healthMax;
            InvokeOnHealthAmountChanged();
        }

        private void Update()
        {
            if (0f < _remainInvincibleTime) _remainInvincibleTime -= Time.deltaTime;
        }

        public bool TryRecovered(int recoverAmount)
        {
            if (IsDead) return false;
            if(_healthMax == _remainHealth) return false;

            AddHealthAmount(recoverAmount);
            PlayRecoveryParticle();

            return true;
        }

        public void OnDetach() => _lip.OnDetach();

        public void OnDamaged(int damageAmount, Vector2 knockback, Vector2 hitPos)
        {
            if (_ignoreDamage) return;

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockback, ForceMode2D.Impulse);

            if (IsInvincible) return;
            if(IsDead) return;

            AddHealthAmount(-damageAmount);

            if (IsDead)
            {
                InvokeOnDead();
            }
            else
            {
                _bodyMove.AddForce(knockback);
                _remainInvincibleTime = _invincibleTime;
            }

            PlayDamagedAudio();
            GenerateDamagedEffect(hitPos);

            OnDamagedEvent?.Invoke();

            if (DeviceVibrator.Instance != null)
            {
                DeviceVibrator.Instance.StartVibrate(_deviceVibrationData);
            }
        }

        private void AddHealthAmount(int add)
        {
            _remainHealth = Mathf.Clamp(_remainHealth + add, 0, _healthMax);

            _spriteChange.ChangeSprites(_remainHealth);
            InvokeOnHealthAmountChanged();
        }

        private void PlayDamagedAudio()
        {
            if(_audioSource == null || _damagedAudioClip == null) return;
            _audioSource.PlayOneShot(_damagedAudioClip);
        }

        private void PlayRecoveryParticle()
        {
            if (_psRecovery == null) return;
            _psRecovery.Play(true);
        }

        private void GenerateDamagedEffect(Vector2 pos)
        {
            if( _damagedEffect == null) return;
            _damagedEffect.GenerateEffect(pos);
        }

        public void SetIgnoreDamage(bool value) => _ignoreDamage = value;

        private void InvokeOnDead() => OnDeadEvent?.Invoke();

        public void InvokeOnHealthAmountChanged() => OnHealthChangedEvent?.Invoke(_remainHealth, _healthMax);
    }
}