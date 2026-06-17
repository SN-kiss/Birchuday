using UnityEngine;

namespace InGame.Camera
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class CameraWide : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _bodyObjName;
        [SerializeField] private float _wideMin;
        [SerializeField] private float _wideMax;
        [SerializeField] private float _widePadding;
        [SerializeField] private float _backgroundScroleWeight;

        [Header("References")]
        [SerializeField] private UnityEngine.Camera _cam;
        [SerializeField] private Transform _targetBGPositionTr;
        [SerializeField] private Transform _targetBGScaleTr;

        private Transform _playerNorthTr;
        private Transform _playerSouthTr;
        private const float _sizeX = 26f;
        private const float _sizeY = 15f;

        private Vector2 PlayerNorthPos
        {
            get
            {
                if (_playerNorthTr == null) return Vector2.zero;
                return _playerNorthTr.position;
            }
        }

        private Vector2 PlayerSouthPos
        {
            get
            {
                if(_playerSouthTr == null) return Vector2.zero;
                return _playerSouthTr.position;
            }
        }

        private void Update()
        {
            if(_playerNorthTr == null) SetPlayerNorthTr(GameObject.FindGameObjectWithTag(_playerNorthTag));
            if (_playerSouthTr == null) SetPlayerSouthTr(GameObject.FindGameObjectWithTag(_playerSouthTag));

            Vector2 centerPoint = (PlayerNorthPos + PlayerSouthPos) * 0.5f;

            float wide = Mathf.Clamp((PlayerNorthPos - PlayerSouthPos).magnitude * 0.5f, _wideMin, _wideMax) + _widePadding;

            transform.position = centerPoint;
            if (_cam != null) _cam.orthographicSize = wide;

            if (_targetBGPositionTr == null || _targetBGScaleTr == null) return;

            float bgScale = wide / (_wideMin + _widePadding);
            float sectionX = centerPoint.x / _sizeX;
            float sectionY = centerPoint.y / _sizeY;
            float bgOffsetX = Mathf.Repeat(sectionX * _backgroundScroleWeight, 1f) * _sizeX;
            float bgOffsetY = Mathf.Repeat(sectionY * _backgroundScroleWeight, 1f) * _sizeY;

            _targetBGPositionTr.localPosition = new Vector2(-bgOffsetX, -bgOffsetY);
            _targetBGScaleTr.localScale = new Vector3(bgScale, bgScale, 1f);
        }

        private void SetPlayerNorthTr(GameObject obj)
        {
            if (obj == null) return;

            if (_playerNorthTr == null)
            {
                Transform[] trs = obj.GetComponentsInChildren<Transform>();
                foreach (Transform t in trs)
                {
                    if (t.gameObject.name == _bodyObjName)
                    {
                        _playerNorthTr = t;
                        Debug.Log("Player North Finded!");
                        return;
                    }
                }
            }
        }

        private void SetPlayerSouthTr(GameObject obj)
        {
            if (obj == null) return;

            if (_playerSouthTr == null)
            {
                Transform[] trs = obj.GetComponentsInChildren<Transform>();
                foreach (Transform t in trs)
                {
                    if (t.gameObject.name == _bodyObjName)
                    {
                        _playerSouthTr = t;
                        Debug.Log("Player South Finded!");
                        return;
                    }
                }
            }
        }
    }
}