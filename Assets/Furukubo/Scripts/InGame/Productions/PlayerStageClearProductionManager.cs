using InGame.Player;
using System.Collections;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerStageClearProductionManager : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private Fade _fade;

        private bool _isStageClear;

        public void OnClearStage()
        {
            if (_isStageClear) return;

            _isStageClear = true;

            PlayerStageClear(_playerNorthTag);
            PlayerStageClear(_playerSouthTag);

            StartCoroutine(ClearStageCoroutine());
            
            IEnumerator ClearStageCoroutine()
            {
                yield return _fade.WaitForEndOfAnimationCoroutine();

                Debug.Log("<color=yellow>Stage Clear!</color>");
            }
        }

        private void PlayerStageClear(string playerTag)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(playerTag);

            if (obj != null)
            {
                Debug.Log($"Found: {obj.name}");

                Transform tr = obj.transform;

                PlayerBodyMove move = tr.GetComponentInChildren<PlayerBodyMove>();
                if (move != null) move.OnClearStage();

                PlayerLip lip = tr.GetComponentInChildren<PlayerLip>();
                if(lip != null) lip.OnClearStage();
            }
        }
    }
}