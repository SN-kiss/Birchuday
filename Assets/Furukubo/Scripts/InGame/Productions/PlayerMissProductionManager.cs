using InGame.Player;
using System.Collections;
using UnityEngine;

namespace InGame
{
    public class PlayerMissProductionManager : MonoBehaviour
    {
        [Header("Player North")]
        [SerializeField] private PlayerBodyMove _moveNorth;
        [SerializeField] private PlayerBodyVibrater _vibraterNorth;
        [SerializeField] private PlayerLip _lipNorth;
        [Header("Player South")]
        [SerializeField] private PlayerBodyMove _moveSouth;
        [SerializeField] private PlayerBodyVibrater _vibraterSouth;
        [SerializeField] private PlayerLip _lipSouth;
        [Header("Others")]
        [SerializeField] private GameObject _msgMissPlayerNorth;
        [SerializeField] private GameObject _msgMissPlayerSouth;

        private bool _isMiss;

        public void OnPlayerNorthMiss()
        {
            if (_isMiss) return;
            _isMiss = true;

            _msgMissPlayerNorth.gameObject.SetActive(true);

            _moveNorth.OnMissStage();
            _moveSouth.OnMissStage();

            _lipNorth.OnMissStage();
            _lipSouth.OnMissStage();

            StartCoroutine(PlayerNorthMissCoroutine());

            IEnumerator PlayerNorthMissCoroutine()
            {
                yield return _vibraterNorth.VibrateCoroutine();

                Debug.Log("<color=red>(North)BOMB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!</color>");

                _msgMissPlayerSouth.gameObject.SetActive(true);
            }
        }

        public void OnPlayerSouthMiss()
        {
            if (_isMiss) return;
            _isMiss = true;

            _moveNorth.OnMissStage();
            _moveSouth.OnMissStage();

            _lipNorth.OnMissStage();
            _lipSouth.OnMissStage();

            StartCoroutine(PlayerSouthMissCoroutine());

            IEnumerator PlayerSouthMissCoroutine()
            {
                yield return _vibraterSouth.VibrateCoroutine();

                Debug.Log("<color=blue>(South)BOMB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!</color>");

                _msgMissPlayerSouth.gameObject.SetActive(true);
            }
        }
    }
}