using InGame.Camera;
using InGame.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame
{
    public class NormalStageClearProductionManager : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _nextSceneName;
        [SerializeField] private CameraTarget _cameraTarget;
        [SerializeField] private Fade _fade;

        private bool _isPlayerNorthStageCleared;
        private bool _isPlayerSouthStageCleared;

        public void OnPlayerNorthClearStage()
        {
            if (_isPlayerNorthStageCleared) return;
            _isPlayerNorthStageCleared = true;

            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerNorthTag));

            _cameraTarget.SetIgnorePlayerNorth();

            if (_isPlayerSouthStageCleared) PlayerBothStageClear();
        }

        public void OnPlayerSouthCleatStage()
        {
            if (_isPlayerSouthStageCleared) return;
            _isPlayerSouthStageCleared = true;

            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerSouthTag));

            _cameraTarget.SetIgnorePlayerSouth();

            if (_isPlayerNorthStageCleared) PlayerBothStageClear();
        }

        private void PlayerStageClear(GameObject player)
        {
            if (player == null) return;

            Debug.Log($"Found: {player.name}");

            Transform tr = player.transform;

            PlayerBodyMove move = tr.GetComponentInChildren<PlayerBodyMove>();
            if (move != null)
            {
                move.OnClearStage();
                move.AddForceImpulse(Vector2.right * 20f);
            }

            PlayerLip lip = tr.GetComponentInChildren<PlayerLip>();
            if (lip != null) lip.OnClearStage();
        }

        private void PlayerBothStageClear()
        {
            StartCoroutine(WaitForEndOfFadeOut());
            
            IEnumerator WaitForEndOfFadeOut()
            {
                Debug.Log(_fade);
                
                if (_fade == null) yield break;

                yield return _fade.WaitForEndOfAnimationCoroutine();

                TransitionToNextScene();
            }
        }

        private void TransitionToNextScene()
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}