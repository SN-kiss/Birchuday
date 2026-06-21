using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace InGame.Enemy
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class EnemyMeteor : MonoBehaviour, IBlackHoleTarget
    {
        [SerializeField] private string _wallTag;
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _trTexture;
        [SerializeField] private TrailRenderer[] _trails;

        public event Action OnReleaseToPool;
        public event Action<Vector2> OnGenerateEffect;
        private bool _rotatePlus;

        public Vector2 Position => transform.position;

        private void Update()
        {
            if (_trTexture == null) return;
            float deltaRot = _rotateSpeed * (_rotatePlus ? 1f : -1f) * Time.deltaTime;
            _trTexture.localEulerAngles += new Vector3(0f, 0f, deltaRot);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageTarget target))
            {
                target.OnDamaged(_damageAmount, _rb.linearVelocity);//dammy
                target.OnDetach();
                Explode();
            }
            else if(collision.gameObject.tag == _wallTag)
            {
                Explode();
            }
        }

        public void OnShot(Vector2 pos, Vector2 initForce)
        {
            _rotatePlus = Random.value < 0.5f;

            transform.position = pos;

            if (_rb != null)
            {
                _rb.position = pos;
                _rb.AddForce(initForce, ForceMode2D.Impulse);
            }

            if (_trails == null) return;
            foreach (var trail in _trails)
            {
                if(trail == null) continue;
                trail.Clear();
            }
        }

        private void Explode()
        {
            OnGenerateEffect?.Invoke(_rb.position);

            if (OnReleaseToPool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                OnReleaseToPool.Invoke();
            }
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force, ForceMode2D.Force);
    }
}