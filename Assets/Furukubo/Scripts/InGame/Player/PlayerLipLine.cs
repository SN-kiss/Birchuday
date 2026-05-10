using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of Line_GOD)
    /// </summary>
    public class PlayerLipLine : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int _lineSegments;//20;
        [SerializeField] private float _waveAmplitude;//0.1f;
        [SerializeField] private float _waveSpeed;//3f;

        [Header("References")]
        [SerializeField] private Transform _bodyTr;
        [SerializeField] private Transform _lipTr;
        [SerializeField] private LineRenderer _line;

        private Vector2 BodyPoint => _bodyTr?.position ?? Vector2.zero;
        private Vector2 LipPoint => _lipTr?.position ?? Vector2.zero;

        private void Start()
        {
            _line.positionCount = _lineSegments;
        }
        
        private void Update()
        {
            if(_line == null) return;

            for (int i = 0; i < _lineSegments; i++)
            {
                float t = (float)i / (_lineSegments - 1);

                Vector3 pos = Vector3.Lerp(BodyPoint, LipPoint, t);

                float wave = 
                    Mathf.Sin(t * Mathf.PI + Time.time * _waveSpeed)
                    * _waveAmplitude * (1 - Mathf.Abs(t - 0.5f) * 2);

                Vector3 perp = Vector3.Cross((LipPoint - BodyPoint).normalized, Vector3.forward);

                pos += perp * wave;

                _line.SetPosition(i, pos);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (_bodyTr == null || _lipTr == null) return;

                _line.positionCount = 2;
                _line.SetPosition(0, _bodyTr.position);
                _line.SetPosition(1, _lipTr.position);
            }
#endif
        }
    }
}