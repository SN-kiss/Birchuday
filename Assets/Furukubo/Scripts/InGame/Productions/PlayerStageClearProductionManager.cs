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
        [Header("Player North")]
        [SerializeField] private PlayerBodyMove _moveNorth;
        [SerializeField] private PlayerLip _lipNorth;
        [Header("Player South")]
        [SerializeField] private PlayerBodyMove _moveSouth;
        [SerializeField] private PlayerLip _lipSouth;
        [Header("Others")]
        [SerializeField] private Fade _fade;

        private bool _isStageClear;

        public void OnClearStage()
        {
            if (_isStageClear) return;

            _isStageClear = true;

            _moveNorth.OnClearStage();
            _moveSouth.OnClearStage();

            _lipNorth.OnClearStage();
            _lipSouth.OnClearStage();

            StartCoroutine(ClearStageCoroutine());
            
            IEnumerator ClearStageCoroutine()
            {
                yield return _fade.WaitForEndOfAnimationCoroutine();

                Debug.Log("<color=yellow>Stage Clear!</color>");
            }
        }
    }
}