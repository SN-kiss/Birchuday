using InGame.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class StageBlackHoleMissProductionManager : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private float _waitTimeToStartFadeOut;
        [SerializeField] private EffectBlackHoleDead _playerNorthBlackHoleEffectPrefab;
        [SerializeField] private EffectBlackHoleDead _playerSouthBlackHoleEffectPrefab;
        [SerializeField] private Fade _fade;

        private GameObject _playerNorth;
        private GameObject _playerSouth;
        private bool _isMiss;

        private void Update()
        {
            if (_playerNorth == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(_playerNorthTag);
                if (obj != null)
                {
                    _playerNorth = obj;

                    PlayerBodyMove m = obj.GetComponentInChildren<PlayerBodyMove>();
                    if (m != null) m._onBlackHoleDead += PlayerNorthBlackHoleDead;

                    Debug.Log($"Found: {_playerNorth.name}");
                }
            }

            if (_playerSouth == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(_playerSouthTag);
                if (obj != null)
                {
                    _playerSouth = obj;

                    PlayerBodyMove m = obj.GetComponentInChildren<PlayerBodyMove>();
                    if (m != null) m._onBlackHoleDead += PlayerSouthBlackHoleDead;

                    Debug.Log($"Found: {_playerSouth.name}");
                }
            }
        }

        private void PlayerNorthBlackHoleDead(Vector2 blackholePos)
        {
            PlayerStageMiss(_playerNorth);
            PlayerStageMiss(_playerSouth);

            _playerNorth.SetActive(false);

            PlayerBodyMove m = _playerNorth.GetComponentInChildren<PlayerBodyMove>();

            Instantiate(_playerNorthBlackHoleEffectPrefab).StartBlackHoleAttracted(
                m.Position,
                m.Velocity,
                blackholePos,
                m.Rotation);

            if (_isMiss) return;
            _isMiss = true;

            StartCoroutine(MissCoroutine());

            IEnumerator MissCoroutine()
            {
                yield return new WaitForSeconds(_waitTimeToStartFadeOut);

                yield return WaitForEndOfFadeOut();

                ReloadCurrentScene();
            }
        }

        private void PlayerSouthBlackHoleDead(Vector2 blackholePos)
        {
            PlayerStageMiss(_playerNorth);
            PlayerStageMiss(_playerSouth);

            _playerSouth.SetActive(false);

            PlayerBodyMove m = _playerSouth.GetComponentInChildren<PlayerBodyMove>();

            Instantiate(_playerSouthBlackHoleEffectPrefab).StartBlackHoleAttracted(
                m.Position,
                m.Velocity,
                blackholePos,
                m.Rotation);

            if (_isMiss) return;
            _isMiss = true;

            StartCoroutine(MissCoroutine());

            IEnumerator MissCoroutine()
            {
                yield return new WaitForSeconds(_waitTimeToStartFadeOut);

                yield return WaitForEndOfFadeOut();

                ReloadCurrentScene();
            }
        }

        private void PlayerStageMiss(GameObject player)
        {
            if (player == null) return;

            PlayerBodyMove move = player.GetComponentInChildren<PlayerBodyMove>();
            if (move != null) move.OnMissStage();

            PlayerLip lip = player.GetComponentInChildren<PlayerLip>();
            if (lip != null) lip.OnMissStage();

            PlayerBodyHealth health = player.GetComponentInChildren<PlayerBodyHealth>();
            if(health != null) health.SetIgnoreDamage(true);
        }

        private IEnumerator WaitForEndOfFadeOut()
        {
            if (_fade == null) yield break;
            yield return _fade.WaitForEndOfAnimationCoroutine();
        }

        private void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}