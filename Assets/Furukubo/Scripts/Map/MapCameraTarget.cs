using UnityEngine;

namespace Map
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class MapCameraTarget : MonoBehaviour
    {
        [Header("Move Range Values")]
        [SerializeField] private float _xMax;
        [SerializeField] private float _xMin;
        [SerializeField] private float _yMax;
        [SerializeField] private float _yMin;
        [Header("--------")]
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _bodyObjName;
        [SerializeField] private Rigidbody2D _rb2D;

        private Transform _playerNorthTr;
        private Transform _playerSouthTr;
        private Vector2 _position;

        private void Update()
        {
            if (_playerNorthTr == null) SetPlayerNorthBodyTr(GameObject.FindGameObjectWithTag(_playerNorthTag));
            if (_playerSouthTr == null) SetPlayerSouthBodyTr(GameObject.FindGameObjectWithTag(_playerSouthTag));

            Vector2 northPos = _playerNorthTr == null ? Vector2.zero : _playerNorthTr.position;
            Vector2 southPos = _playerSouthTr == null ? Vector2.zero : _playerSouthTr.position;

            Vector2 averagePos = (northPos + southPos) * 0.5f;

            _position =
                new Vector2(
                    Mathf.Clamp(averagePos.x, _xMin, _xMax),
                    Mathf.Clamp(averagePos.y, _yMin, _yMax));
        }

        private void FixedUpdate()
        {
            _rb2D.MovePosition(_position);
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

        private void OnDrawGizmos()
        {
            Vector2 leftUp = new Vector2(_xMin, _yMax);
            Vector2 rightUp = new Vector2(_xMax, _yMax);
            Vector2 rightDown = new Vector2(_xMax, _yMin);
            Vector2 leftDown = new Vector2(_xMin, _yMin);

            OriginalGizmoUtils.DrawBox(leftUp, rightUp, rightDown, leftDown, Color.red);

            OriginalGizmoUtils.DrawCross(leftUp, 0.5f, Color.red);
            OriginalGizmoUtils.DrawCross(rightUp, 0.5f, Color.red);
            OriginalGizmoUtils.DrawCross(rightDown, 0.5f, Color.red);
            OriginalGizmoUtils.DrawCross(leftDown, 0.5f, Color.red);
        }
    }
}