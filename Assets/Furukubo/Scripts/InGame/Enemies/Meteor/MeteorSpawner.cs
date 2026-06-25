using InGame.Effect;
using UnityEngine;
using UnityEngine.Pool;

namespace InGame.Enemy
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class MeteorSpawner : MonoBehaviour
    {
        [Header("Paramters")]
        [SerializeField, Min(0.05f)] private float _spawnIntervalTime;
        [SerializeField] private float _angle;
        [SerializeField, Min(0f)] private float _power;
        [Header("References")]
        [SerializeField] private EnemyMeteor _meteorPrefab;
        [SerializeField] private EffectGenerator _effectGenerator;

        private ObjectPool<EnemyMeteor> _meteorPool;
        private float _spawnIntervalTimeCount;
        private bool _isStop;

        private void Awake()
        {
            _meteorPool = new ObjectPool<EnemyMeteor>(
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

            _meteorPool.Get().OnShot(
                transform.position,
                OriginalCalculateUtils.AngleToDirection(_angle) * _power);
        }

        private void OnGenerateEffect(Vector2 pos)
        {
            if (_effectGenerator == null) return;
            _effectGenerator.GenerateEffect(pos);
        }

        private EnemyMeteor InstantiateNewMeteor()
        {
            EnemyMeteor m = Instantiate(_meteorPrefab);
            m.OnReleaseToPool += (M) => ReleaseMeteorToPool(M);
            m.OnGenerateEffect += OnGenerateEffect;
            m.transform.SetParent(transform);
            return m;
        }

        private void ReleaseMeteorToPool(EnemyMeteor meteor)
        {
            _meteorPool?.Release(meteor);
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