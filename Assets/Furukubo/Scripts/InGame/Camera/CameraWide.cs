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
        [SerializeField] private Transform _targetBGPositionTr;
        [SerializeField] private Transform _targetBGScaleTr;
        [SerializeField] private CameraTarget _cameraTarget;

        private const float _SizeX = 26f;
        private const float _SizeY = 15f;

        private void Update()
        {
            if (_cameraTarget == null) return;

            Vector2 northPos = _cameraTarget.NorthPos;
            Vector2 southPos = _cameraTarget.SouthPos;

            Vector2 centerPoint = (northPos + southPos) * 0.5f;
            float wide = Mathf.Clamp((northPos - southPos).magnitude * 0.5f, _wideMin, _wideMax) + _widePadding;

            transform.position = centerPoint;
            if (_cam != null) _cam.orthographicSize = wide;

            if (_targetBGPositionTr == null || _targetBGScaleTr == null) return;

            float bgScale = wide / (_wideMin + _widePadding);
            float sectionX = centerPoint.x / _SizeX;
            float sectionY = centerPoint.y / _SizeY;
            float bgOffsetX = Mathf.Repeat(sectionX * _backgroundScroleWeight, 1f) * _SizeX;
            float bgOffsetY = Mathf.Repeat(sectionY * _backgroundScroleWeight, 1f) * _SizeY;

            _targetBGPositionTr.localPosition = new Vector2(-bgOffsetX, -bgOffsetY);
            _targetBGScaleTr.localScale = new Vector3(bgScale, bgScale, 1f);
        }
    }
}