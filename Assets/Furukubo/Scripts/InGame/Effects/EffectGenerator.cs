using UnityEngine;
using UnityEngine.Pool;

namespace InGame.Effect
{
    public class EffectGenerator : MonoBehaviour
    {
        [SerializeField] private EffectControler _prefab;

        private ObjectPool<EffectControler> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<EffectControler>(
                () => InstantiateNewEffect(),
                (e) => e.gameObject.SetActive(true),
                (e) => e.gameObject.SetActive(false));
        }

        public void GenerateEffect(Vector2 pos) => _pool?.Get().OnGenerated(pos);

        private EffectControler InstantiateNewEffect()
        {
            EffectControler e = Instantiate(_prefab);
            e.OnReleaseToPool += () => ReleaseToPool(e);
            e.transform.SetParent(transform);
            return e;
        }

        private void ReleaseToPool(EffectControler effect) => _pool?.Release(effect);
    }
}