using InGame.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class FinalStageClearProductionManager : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _nextSceneName;
        [SerializeField] private Fade _fade;

        private bool _isStageCleared;

        public void OnClearStage()
        {
            if (_isStageCleared) return;
            _isStageCleared = true;

            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerNorthTag));
            PlayerStageClear(GameObject.FindGameObjectWithTag(_playerSouthTag));

            StartCoroutine(ClearStageCoroutine());
            
            IEnumerator ClearStageCoroutine()
            {
                yield return WaitForEndOfFadeOut();

                Debug.Log("<color=yellow>Stage Clear!</color>");

                TransitionToNextScene();
            }
        }

        private void PlayerStageClear(GameObject player)
        {
            if (player == null) return;

            Debug.Log($"Found: {player.name}");

            Transform tr = player.transform;

            PlayerBodyMove move = tr.GetComponentInChildren<PlayerBodyMove>();
            if (move != null) move.OnClearStage();

            PlayerLip lip = tr.GetComponentInChildren<PlayerLip>();
            if (lip != null) lip.OnClearStage();

            PlayerBodyHealth health = player.GetComponentInChildren<PlayerBodyHealth>();
            if (health != null) health.SetIgnoreDamage(true);
        }

        private IEnumerator WaitForEndOfFadeOut()
        {
            Debug.Log(_fade);
            if (_fade == null) yield break;
            yield return _fade.WaitForEndOfAnimationCoroutine();
        }

        private void TransitionToNextScene()
        {
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}