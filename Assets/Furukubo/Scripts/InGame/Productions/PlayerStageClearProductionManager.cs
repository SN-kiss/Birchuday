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
        [SerializeField] private GameObject _msgStageClear;
        [SerializeField] private Animator _animFlash;
        [SerializeField] private string _flashAnimStateName;

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
                _animFlash.gameObject.SetActive(true);
                _animFlash.Play(_flashAnimStateName);

                yield return new WaitUntil(() => _animFlash.GetCurrentAnimatorStateInfo(0).IsName(_flashAnimStateName));

                yield return new WaitUntil(() => 1f <= _animFlash.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _msgStageClear.SetActive(true);
            }
        }
    }
}