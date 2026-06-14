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
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private PlayerBodyMove _bodyMove;
        [SerializeField] private PlayerLip _lip;
        [SerializeField] private UnityEvent<int, int> _onHealthChanged;
        [SerializeField] private UnityEvent _onDead;

        private int _remainHealth;
        private float _remainInvincibleTime;

        public Vector2 Position => _rb.position;
        private bool IsInvincible => 0f < _remainInvincibleTime;
        private bool IsDead => _remainHealth <= 0;

        private void Start()
        {
            _remainHealth = _healthMax;
            _onHealthChanged?.Invoke(_remainHealth, _healthMax);
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
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockback, ForceMode2D.Impulse);

            if (IsInvincible) return;
            if(IsDead) return;

            AddHealthAmount(-damageAmount);

            if (IsDead)
            {
                _onDead?.Invoke();
            }
            else
            {
                _bodyMove.AddForce(knockback);
                _remainInvincibleTime = _invincibleTime;
            }
        }

        private void AddHealthAmount(int add)
        {
            _remainHealth = Mathf.Clamp(_remainHealth + add, 0, _healthMax);

            _onHealthChanged?.Invoke(_remainHealth, _healthMax);
        }
    }
}