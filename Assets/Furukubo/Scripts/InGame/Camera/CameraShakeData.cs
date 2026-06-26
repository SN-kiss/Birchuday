using UnityEngine;

namespace InGame.Camera
{
    /// <summary>
    /// Furukubo
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/Camera Shake Data")]
    public class CameraShakeData : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _shakeSpeed;
        [SerializeField, Min(0f)] private float _shakeRadius;
        [SerializeField, Min(0f)] private float _shakeDamping;
        [SerializeField, Min(0f)] private float _noiseScale;
        [SerializeField] private Vector2 _shakeScale;

        public float ShakeSpeed => _shakeSpeed;
        public float ShakeRadius => _shakeRadius;
        public float ShakeDamping => _shakeDamping;
        public float NoiseScale => _noiseScale;
        public Vector2 ShakeScale => _shakeScale;
    }
}