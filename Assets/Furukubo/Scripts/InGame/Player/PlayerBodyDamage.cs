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

        public void OnDamaged(int damage, Vector2 nockback)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(nockback, ForceMode2D.Impulse);

            _onDamaged?.Invoke();

            if (IsInvincible) return;
            if(IsDead) return;

            _remainHealth = Mathf.Clamp(_remainHealth - damage, 0, _healthMax);

            if (IsDead)
            {
                _onDead?.Invoke();
                _srBody.color = Color.gray;
            }
            else
            {
                _remainInvincibleTime = _invincibleTime;
            }
        }
    }
}