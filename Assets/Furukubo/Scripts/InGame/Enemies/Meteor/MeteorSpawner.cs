using UnityEngine;
using UnityEngine.Pool;

namespace InGame.Enemy
{
    public class MeteorSpawner : MonoBehaviour
    {
        [SerializeField, Min(0.05f)] private float _spawnIntervalTime;
        [SerializeField] private float _angle;
        [SerializeField, Min(0f)] private float _power;
        [SerializeField] private EnemyMeteor _meteorPrefab;

        private ObjectPool<EnemyMeteor> _pool;
        private float _spawnIntervalTimeCount;
        private bool _isStop;

        private void Awake()
        {
            _pool = new ObjectPool<EnemyMeteor>(
                () => InstantiateNewMeteor(),
                (m) => m.gameObject.SetActive(true),
                (m) => m.gameObject.SetActive(false));
        }

        public void Update()
        {
            if(_isStop) return;

            _spawnIntervalTimeCount += Time.deltaTime;

            if (_spawnIntervalTime <= _spawnIntervalTimeCount)
            {
                _spawnIntervalTimeCount = 0f;

                OnSpawn();
            }
        }

        public void Stop() => _isStop = true;
        public void Play() => _isStop = false;

        public void OnSpawn()
        {
            if (_meteorPrefab == null) return;

            _pool.Get().OnShot(
                transform.position,
                OriginalCalculateUtils.AngleToDirection(_angle) * _power);
        }

        private EnemyMeteor InstantiateNewMeteor()
        {
            EnemyMeteor m = Instantiate(_meteorPrefab);
            m.OnReleaseToPool += () => _pool?.Release(m);
            return m;
        }

        private void OnDrawGizmos()
        {
            Vector2 pos = transform.position;

            OriginalGizmoUtils.DrawArrow(
                pos,
                pos + OriginalCalculateUtils.AngleToDirection(_angle) * _power,
                Color.cyan);

            OriginalGizmoUtils.DrawStar(pos, 5, 1f, 0.5f, Color.cyan);
        }
    }
}