using InGame.Effect;
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
        [SerializeField] private float _waitingExplodeTime;
        [SerializeField] private float _waitingFadeTime;
        [SerializeField] private Fade _fade;
        [SerializeField] private EffectControler _explosionEffectPrefab;

        private bool _isMiss;

        public void OnPlayerNorthMiss()
        {
            if (_isMiss) return;
            _isMiss = true;

            _moveNorth.OnMissStage();
            _moveSouth.OnMissStage();

            _lipNorth.OnMissStage();
            _lipSouth.OnMissStage();

            StartCoroutine(PlayerNorthMissCoroutine());

            IEnumerator PlayerNorthMissCoroutine()
            {
                yield return _vibraterNorth.VibrateCoroutine();

                yield return new WaitForSeconds(_waitingExplodeTime);

                _moveNorth.transform.parent.gameObject.SetActive(false);
                GenerateExplodeEffect(_moveNorth.Position);
                Debug.Log("<color=red>(North)BOMB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!</color>");

                yield return new WaitForSeconds(_waitingFadeTime);

                yield return _fade.WaitForEndOfAnimationCoroutine();
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

                yield return new WaitForSeconds(_waitingExplodeTime);

                _moveSouth.transform.parent.gameObject.SetActive(false);
                GenerateExplodeEffect(_moveSouth.Position);
                Debug.Log("<color=blue>(South)BOMB!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!</color>");

                yield return new WaitForSeconds(_waitingFadeTime);

                yield return _fade.WaitForEndOfAnimationCoroutine();
            }
        }

        private void GenerateExplodeEffect(Vector2 pos)
        {
            Instantiate(_explosionEffectPrefab).OnGenerated(pos);
        }
    }
}