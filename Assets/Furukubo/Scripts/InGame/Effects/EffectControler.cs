using System;
using UnityEngine;

namespace InGame.Effect
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class EffectControler : MonoBehaviour
    {
        public event Action OnReleaseToPool;

        private ParticleSystem[] _pss;
        private bool _disposed;

        private void Awake()
        {
            _pss = GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (_pss == null) return;

            foreach (ParticleSystem ps in _pss)
            {
                if (ps != null && ps.IsAlive(true)) return;
            }

            if (!_disposed)
            {
                _disposed = true;

                if (OnReleaseToPool == null)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    OnReleaseToPool.Invoke();
                }
            }
        }

        public void OnGenerated(Vector2 pos)
        {
            transform.position = pos;
            _disposed = false;
        }
    }
}