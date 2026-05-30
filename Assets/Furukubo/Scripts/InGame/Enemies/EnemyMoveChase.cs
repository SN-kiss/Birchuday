using UnityEngine;

namespace InGame.Enemy
{
    public class EnemyMoveChase : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _missDistance;
        [SerializeField] private float _chasePower;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private GameObject _objSearchArea;

        private IDamageTarget _target;

        public void OnHitSearchCollider(Collider2D col)
        {
            if (_target != null) return;

            if (col.TryGetComponent(out IDamageTarget target))
            {
                _target = target;
                _objSearchArea.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            Vector2 delta = _target.Position - _rb.position;

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