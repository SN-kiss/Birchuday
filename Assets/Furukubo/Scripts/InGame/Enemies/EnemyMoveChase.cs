using System.Collections.Generic;
using UnityEngine;

namespace InGame.Enemy
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class EnemyMoveChase : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _chasePower;
        [SerializeField] private MagneticType _selfMagnetixType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;

        private List<IDamageTarget> _targets;

        private void Awake()
        {
            _targets = new List<IDamageTarget>();
        }

        public void OnHitSearchCollider(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageTarget target))
            {
                if (!MagnetJudgement.IsAttachable(_selfMagnetixType, target.MagneticType)) return;

                if (_targets.Contains(target)) return;

                _targets.Add(target);
            }
        }

        private void FixedUpdate()
        {
            if(_rb == null) return;
            if (_targets == null) return;
            if (_targets.Count == 0) return;

            Vector2 pos = _rb.position;

            IDamageTarget result = null;
            float resultSqrDistance = float.PositiveInfinity;

            foreach (var t in _targets)
            {
                if (t == null) continue;

                float newSqrDistance = (t.Position - pos).sqrMagnitude;

                if (newSqrDistance <= resultSqrDistance)
                {
                    result = t;
                    resultSqrDistance = newSqrDistance;
                }
            }

            if (result == null) return;

            Vector2 dir = (result.Position - pos).normalized;

            _rb.AddForce(dir * _chasePower);
        }

        private void OnDrawGizmos()
        {
            if (_rb == null) return;
            if (_targets == null) return;
            if (_targets.Count == 0) return;

            Vector2 pos = _rb.position;

            IDamageTarget result = null;
            float resultSqrDistance = float.PositiveInfinity;

            foreach (var t in _targets)
            {
                if (t == null) continue;

                float newSqrDistance = (t.Position - pos).sqrMagnitude;

                if (newSqrDistance <= resultSqrDistance)
                {
                    result = t;
                    resultSqrDistance = newSqrDistance;
                }
            }

            if (result == null) return;

            float length = 1.5f;
            Vector2 resultPos = result.Position;

            Debug.DrawLine(resultPos + new Vector2(-length, length), resultPos + new Vector2(length, -length), Color.red);
            Debug.DrawLine(resultPos + new Vector2(length, length), resultPos + new Vector2(-length, -length), Color.red);
        }
    }
}