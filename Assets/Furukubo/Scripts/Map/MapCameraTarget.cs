using UnityEngine;

namespace Map
{
    public class MapCameraTarget : MonoBehaviour
    {
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
            
            _position = (northPos + southPos) * 0.5f;
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
    }
}