using UnityEngine;

namespace InGame.Camera
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _bodyObjName;

        private Transform _playerNorthTr;
        private Transform _playerSouthTr;
        private Vector2 _northPos;
        private Vector2 _southPos;
        private Vector2 _lerpStartPos;
        private TargetIgnoreState _state;
        private float _time;

        public Vector2 NorthPos => _northPos;
        public Vector2 SouthPos => _southPos;

        void Update()
        {
            if (_playerNorthTr == null) SetPlayerNorthBodyTr(GameObject.FindGameObjectWithTag(_playerNorthTag));
            if (_playerSouthTr == null) SetPlayerSouthBodyTr(GameObject.FindGameObjectWithTag(_playerSouthTag));

            if (_playerNorthTr == null || _playerSouthTr == null) return;

            if(_state == TargetIgnoreState.Ordinary)
            {
                _northPos = _playerNorthTr.position;
                _southPos = _playerSouthTr.position;
            }
            else if (_state == TargetIgnoreState.IgnoreNorth)
            {
                _time = Mathf.Clamp01(_time + Time.deltaTime * _lerpSpeed);

                _southPos = _playerSouthTr.position;
                _northPos = Vector2.Lerp(_lerpStartPos, _southPos, _time);
            }
            else if (_state == TargetIgnoreState.IgnoreSouth)
            {
                _time = Mathf.Clamp01(_time + Time.deltaTime * _lerpSpeed);

                _northPos = _playerNorthTr.position;
                _southPos = Vector2.Lerp(_lerpStartPos, _northPos, _time);
            }
            else
            {
                //Do Nothing.
            }
        }

        private void SetPlayerNorthBodyTr(GameObject player)
        {
            if (player == null) return;

            if (_playerNorthTr == null)
            {
                Transform tr = player.transform.Find(_bodyObjName);

                if (tr == null) return;

                _playerNorthTr = tr;

                Debug.Log($"Found: {player.name} / {tr.gameObject.name}");
            }
        }

        private void SetPlayerSouthBodyTr(GameObject player)
        {
            if (player == null) return;

            if (_playerSouthTr == null)
            {
                Transform tr = player.transform.Find(_bodyObjName);

                if (tr == null) return;

                _playerSouthTr = tr;

                Debug.Log($"Found: {player.name} / {tr.gameObject.name}");
            }
        }

        public void SetIgnorePlayerNorth()
        {
            if (_state == TargetIgnoreState.IgnoreBoth) return;

            if (_state == TargetIgnoreState.IgnoreSouth)
            {
                _state = TargetIgnoreState.IgnoreBoth;
            }
            else
            {
                _state = TargetIgnoreState.IgnoreNorth;
                _lerpStartPos = _northPos;
                _time = 0f;
            }
        }

        public void SetIgnorePlayerSouth()
        {
            if (_state == TargetIgnoreState.IgnoreBoth) return;

            if (_state == TargetIgnoreState.IgnoreNorth)
            {
                _state = TargetIgnoreState.IgnoreBoth;
            }
            else
            {
                _state = TargetIgnoreState.IgnoreSouth;
                _lerpStartPos = _southPos;
                _time = 0f;
            }
        }

        private enum TargetIgnoreState
        {
            Ordinary,
            IgnoreNorth,
            IgnoreSouth,
            IgnoreBoth
        }
    }
}