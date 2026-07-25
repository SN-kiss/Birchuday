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
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private MagneticType _selfMagnetixType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Transform _trTex;

        private List<IDamageTarget> _targets;
        private float _sign;

        private void Awake()
        {
            _targets = new List<IDamageTarget>();
            _sign = Random.value < 0.5f ? -1f : 1f;
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

        private void Update()
        {
            float angle = _trTex.localEulerAngles.z;
            float delta = _rb.linearVelocity.magnitude * _rotateSpeed * Time.deltaTime;
            float newAngle = angle + delta * _sign;
            _trTex.localEulerAngles = new Vector3(0f, 0f, newAngle);
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
            if (_maxSpeed * _maxSpeed <= _rb.linearVelocity.sqrMagnitude)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
            }
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