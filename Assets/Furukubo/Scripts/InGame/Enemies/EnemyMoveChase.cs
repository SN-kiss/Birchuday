using UnityEngine;

namespace InGame.Enemy
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class EnemyMoveChase : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _missDistance;
        [SerializeField] private float _chasePower;
        [SerializeField] private MagneticType _selfMagnetixType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private GameObject _objSearchArea;

        private ILip _target;

        public void OnHitSearchCollider(Collider2D col)
        {
            if (_target != null) return;

            if (col.TryGetComponent(out ILip target))
            {
                if (!MagnetJudgement.IsAttachable(_selfMagnetixType, target.MagneticType)) return;

                _target = target;
                _objSearchArea.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            Vector2 delta = _target.LipPosition - _rb.position;

            if (_missDistance * _missDistance <= delta.sqrMagnitude)
            {
                _target = null;
                _objSearchArea.SetActive(true);
            }
            else
            {
                _rb.AddForce(delta.normalized * _chasePower);
            }
        }
    }
}