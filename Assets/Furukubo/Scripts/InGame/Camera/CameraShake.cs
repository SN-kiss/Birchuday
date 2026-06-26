using UnityEngine;

namespace InGame.Camera
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private Transform _shakeTr;

        private CameraShakeData _data;
        private float _curShakeDegree;
        private float _curShakeRadius;
        private bool _curShakeSign;

        private const float _RadiusLimitMin = 0.01f;
        private const float _OneLapDeg = 360f;

        private void Update()
        {
            if(_data == null) return;
            if(_shakeTr == null) return;
            if (_curShakeRadius <= 0f) return;

            float sign = _curShakeSign ? 1f : -1f;
            float deltaDeg = _data.ShakeSpeed * sign * _OneLapDeg * Time.deltaTime;
            _curShakeDegree = Mathf.Repeat(_curShakeDegree + deltaDeg, _OneLapDeg);

            float deltaRadius = _curShakeRadius * _data.ShakeDamping * Time.deltaTime;
            _curShakeRadius -= deltaRadius;

            if(_curShakeRadius <= _RadiusLimitMin)
            {
                _curShakeRadius = 0f;
                _shakeTr.localPosition = Vector3.zero;
            }
            else
            {
                Vector2 offset = OriginalCalculateUtils.AngleToDirection(_curShakeDegree) * _curShakeRadius;

                float radiusRatio = _data.ShakeRadius <= 0f ? 0f : _curShakeRadius / _data.ShakeRadius;
                Vector2 noise = OriginalCalculateUtils.AngleToDirection(Random.value * _OneLapDeg) * _data.NoiseScale * radiusRatio;

                _shakeTr.localPosition = _data.ShakeScale * (offset + noise);
            }
        }

        /// <summary>
        /// Dammy
        /// </summary>
        public void SetShake(CameraShakeData data)
        {
            _data = data;
            _curShakeDegree = Random.value * _OneLapDeg;
            _curShakeRadius = _data.ShakeRadius;
            _curShakeSign = Random.value < 0.5f;
        }
    }
}