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

        [Header("References")]
        [SerializeField] private AudioClip _damagedAudioClip;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerBodyMove _bodyMove;
        [SerializeField] private PlayerLip _lip;
        [SerializeField] private PlayerSpriteChange _spriteChange;
        
        public event Action OnDead;
        public event Action<int, int> OnHealthChanged;
        private int _remainHealth;
        private float _remainInvincibleTime;

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

            return true;
        }

        public void OnDetach() => _lip.OnDetach();

        public void OnDamaged(int damageAmount, Vector2 knockback)
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
        }

        private void AddHealthAmount(int add)
        {
            _remainHealth = Mathf.Clamp(_remainHealth + add, 0, _healthMax);

            _spriteChange.ChangeSprites(_remainHealth);
            InvokeOnHealthAmountChanged();
        }

        private void PlayDamagedAudio() => _audioSource.PlayOneShot(_damagedAudioClip);

        public void SetIgnoreDamage(bool value) => _ignoreDamage = value;

        private void InvokeOnDead() => OnDead?.Invoke();

        public void InvokeOnHealthAmountChanged() => OnHealthChanged?.Invoke(_remainHealth, _healthMax);
    }
}