using InGame.Player;
using UnityEngine;

namespace InGame.Camera
{
    public class CameraPlayerDamagedVibrationInitializer : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private CameraShakeData _damagedCamShakeData;
        [SerializeField] private CameraShake _cameraShake;

        private bool _isNorthExist;
        private bool _isSouthExist;

        private void Update()
        {
            if (!_isNorthExist) SetPlayerNorthBodyTr(GameObject.FindGameObjectWithTag(_playerNorthTag));
            if (!_isSouthExist) SetPlayerSouthBodyTr(GameObject.FindGameObjectWithTag(_playerSouthTag));
        }

        private void SetPlayerNorthBodyTr(GameObject player)
        {
            if (player == null) return;

            PlayerBodyHealth health = player.transform.GetComponentInChildren<PlayerBodyHealth>();

            if (health == null) return;

            health.OnDamagedEvent += ShakeCamera;
            _isNorthExist = true;
        }

        private void SetPlayerSouthBodyTr(GameObject player)
        {
            if (player == null) return;

            PlayerBodyHealth health = player.transform.GetComponentInChildren<PlayerBodyHealth>();

            if (health == null) return;

            health.OnDamagedEvent += ShakeCamera;
            _isSouthExist = true;
        }

        private void ShakeCamera()
        {
            if (_cameraShake == null) return;
            _cameraShake.SetShake(_damagedCamShakeData);
        }
    }
}