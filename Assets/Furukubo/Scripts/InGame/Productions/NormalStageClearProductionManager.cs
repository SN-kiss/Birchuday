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

            if (StageEntryInfo.Instance != null)
            {
                StageEntryInfo.Instance.SetEntryState(StageEntryState.Clear);
            }

            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerNorthTag));

            if(_cameraTarget != null) _cameraTarget.SetIgnorePlayerNorth();

            if (_isPlayerSouthStageCleared) PlayerBothStageClear();
        }

        public void OnPlayerSouthCleatStage()
        {
            if (_isPlayerSouthStageCleared) return;
            _isPlayerSouthStageCleared = true;

            if (StageEntryInfo.Instance != null)
            {
                StageEntryInfo.Instance.SetEntryState(StageEntryState.Clear);
            }

            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerSouthTag));

            if (_cameraTarget != null) _cameraTarget.SetIgnorePlayerSouth();

            if (_isPlayerNorthStageCleared) PlayerBothStageClear();
        }

        private void PlayerStageClear(GameObject player)
        {
            if (player == null) return;

            Debug.Log($"Found: {player.name}");

            PlayerBodyMove move = player.GetComponentInChildren<PlayerBodyMove>();
            if (move != null)
            {
                move.SetIgnoreInput(true);
                move.AddForceImpulse(Vector2.right * 20f);
            }

            PlayerLip lip = player.GetComponentInChildren<PlayerLip>();
            if (lip != null) lip.OnClearStage();

            PlayerBodyHealth health = player.GetComponentInChildren<PlayerBodyHealth>();
            if (health != null) health.SetIgnoreDamage(true);

            //PlayerNormalGoalTrigger trigger = player.GetComponentInChildren<PlayerNormalGoalTrigger>();
            //if (trigger != null) trigger.SetIgnoreGoal(true);
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