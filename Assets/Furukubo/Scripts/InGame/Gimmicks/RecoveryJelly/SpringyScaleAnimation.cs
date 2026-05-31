using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class SpringyScaleAnimation : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField, Range(0.05f, 0.95f)] private float _springRatio;
        [SerializeField] private float _springSpeed;
        [SerializeField] private float _springDamping;

        [Header("References")]
        [SerializeField] private SpriteRenderer _targetSr;

        private Vector2 _defaultSize;
        private float _ratio;
        private float _time;

        private void Start()
        {
            if (_targetSr == null) return;

            _defaultSize = _targetSr.size;
        }

        private void Update()
        {
            if (0f < _ratio)
            {
                if (_targetSr == null) return;

                _time += Time.deltaTime * _springSpeed;

                float defaultX = _defaultSize.x;
                float defaultY = _defaultSize.y;

                float sin = Mathf.Sin(_time * Mathf.PI) * defaultX * _springRatio * _ratio * _ratio;
                float springedX = defaultX + sin;

                if (springedX <= 0f) return;

                float springedY = defaultX * defaultY / springedX;

                _targetSr.size = new Vector2(springedX, springedY);

                _ratio = _ratio - Time.deltaTime * _ratio * _springDamping;

                if (_ratio <= 0.05f)
                {
                    _targetSr.size = _defaultSize;
                }
            }
        }

        public void OnSpring()
        {
            _ratio = 1f;
            _time = Random.Range(0f, 2f);
        }
    }
}