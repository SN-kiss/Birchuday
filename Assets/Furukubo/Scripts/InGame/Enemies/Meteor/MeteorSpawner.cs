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
        [SerializeField] private EffectBlackHoleDead _blackHoleEffectPrefab;

        private ObjectPool<EnemyMeteor> _meteorPool;
        private ObjectPool<EffectBlackHoleDead> _blackHoleEffectPool;
        private float _spawnIntervalTimeCount;
        private bool _isStop;

        private void Awake()
        {
            _meteorPool = new ObjectPool<EnemyMeteor>(
                () => InstantiateNewMeteor(),
                (m) => m.gameObject.SetActive(true),
                (m) => m.gameObject.SetActive(false));

            _blackHoleEffectPool = new ObjectPool<EffectBlackHoleDead>(
                () => InstantiateNewBlackHoleEffect(),
                (e) => e.gameObject.SetActive(true),
                (e) => e.gameObject.SetActive(false));
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

        private void OnDrawGizmos()
        {
            Vector2 pos = transform.position;

            OriginalGizmoUtils.DrawArrow(
                pos,
                pos + OriginalCalculateUtils.AngleToDirection(_angle) * _power,
                Color.cyan);

            OriginalGizmoUtils.DrawStar(pos, 5, 1f, 0.5f, Color.cyan);
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

        private void OnGenerateExplosionEffect(Vector2 pos)
        {
            if (_effectGenerator == null) return;
            _effectGenerator.GenerateEffect(pos);
        }

        private EnemyMeteor InstantiateNewMeteor()
        {
            EnemyMeteor instance = Instantiate(_meteorPrefab);
            instance.OnReleaseToPool += (m) => ReleaseMeteorToPool(m);
            instance.OnGenerateExplosionEffect += OnGenerateExplosionEffect;
            instance.OnGenerateBlackHoleEffect += OnGenerateBackHoleEffect;
            instance.transform.SetParent(transform);
            return instance;
        }

        private void ReleaseMeteorToPool(EnemyMeteor meteor)
        {
            _meteorPool?.Release(meteor);
        }

        //-------------------------------------------------------------------------

        private void OnGenerateBackHoleEffect(Vector2 posS, Vector2 velo, Vector2 posG, float rot)
        {
            if (_blackHoleEffectPrefab == null) return;
            _blackHoleEffectPool.Get().StartBlackHoleAttracted(posS, velo, posG, rot);
        }

        private EffectBlackHoleDead InstantiateNewBlackHoleEffect()
        {
            EffectBlackHoleDead instance = Instantiate(_blackHoleEffectPrefab);
            instance.OnReleaseToPool += (e) => { ReleaseBlackHoleEffectToPool(e); };
            instance.transform.parent = transform;
            return instance;
        }

        private void ReleaseBlackHoleEffectToPool(EffectBlackHoleDead effect)
        {
            _blackHoleEffectPool.Release(effect);
        }
    }
}