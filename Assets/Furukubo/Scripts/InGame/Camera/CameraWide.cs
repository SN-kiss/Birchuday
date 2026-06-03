using UnityEngine;

namespace InGame.Camera
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class CameraWide : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _wideMin;
        [SerializeField] private float _wideMax;
        [SerializeField] private float _widePadding;
        [SerializeField] private float _backgroundScroleWeight;

        [Header("References")]
        [SerializeField] private UnityEngine.Camera _cam;
        [SerializeField] private Transform _tr;
        [SerializeField] private Transform _targetATr;
        [SerializeField] private Transform _targetBTr;
        [SerializeField] private Transform _targetBackgroundTr;

        private const float _sizeX = 26f;
        private const float _sizeY = 15f;

        private Vector2 PointA
        {
            get
            {
                if (_targetATr == null) return Vector2.zero;
                return _targetATr.position;
            }
        }

        private Vector2 PointB
        {
            get
            {
                if(_targetBTr == null) return Vector2.zero;
                return _targetBTr.position;
            }
        }

        private void Update()
        {
            Vector2 centerPoint = (PointA + PointB) * 0.5f;

            float wide = Mathf.Clamp((PointA - PointB).magnitude * 0.5f, _wideMin, _wideMax) + _widePadding;

            if(_tr != null) _tr.position = centerPoint;
            if (_cam != null) _cam.orthographicSize = wide;

            if(_targetBackgroundTr != null)
            {
                float bgScale = wide / (_wideMin + _widePadding);

                float sectionX = centerPoint.x / _sizeX;
                float sectionY = centerPoint.y / _sizeY;

                float bgOffsetX = Mathf.Repeat(sectionX * _backgroundScroleWeight, 1f) * _sizeX * bgScale;
                float bgOffsetY = Mathf.Repeat(sectionY * _backgroundScroleWeight, 1f) * _sizeY * bgScale;

                _targetBackgroundTr.localPosition = new Vector2(-bgOffsetX, -bgOffsetY);
                _targetBackgroundTr.localScale = new Vector3(bgScale, bgScale, 1f);
            }
        }
    }
}