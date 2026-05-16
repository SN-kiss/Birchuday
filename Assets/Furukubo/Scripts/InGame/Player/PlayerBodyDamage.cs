using UnityEngine;
using UnityEngine.Events;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerBodyDamage : MonoBehaviour, IDamageTarget
    {
        [Header("Parameters")]
        [SerializeField] private int _healthMax;
        [SerializeField] private float _invincibleTime;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _srBody;
        [SerializeField] private UnityEvent _onDamaged;
        [SerializeField] private UnityEvent _onDead;

        private int _remainHealth;
        private float _remainInvincibleTime;

        public Vector2 Position => _rb.position;
        private bool IsInvincible => 0f < _remainInvincibleTime;
        private bool IsDead => _remainHealth <= 0;

        private void Awake()
        {
            _remainHealth = _healthMax;
        }

        private void Update()
        {
            //dammy
            if(IsInvincible)
            {
                _remainInvincibleTime -= Time.deltaTime;

                if (IsInvincible)
                {
                    _srBody.color = Color.blue;
                }
                else
                {
                    _srBody.color = Color.red;
                }
            }
        }

        public void OnDamaged(int damage)
        {
            if (IsInvincible) return;
            if(IsDead) return;

            _remainHealth = Mathf.Clamp(_remainHealth - damage, 0, _healthMax);

            _onDamaged?.Invoke();

            if (IsDead)
            {
                _onDead?.Invoke();
                _srBody.color = Color.gray;
            }
            else
            {
                SetInvincibleTime();
            }
        }

        public void OnNockBack(Vector2 force)
        {
            if (IsDead) return;
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void SetInvincibleTime() => _remainInvincibleTime = _invincibleTime;
    }
}