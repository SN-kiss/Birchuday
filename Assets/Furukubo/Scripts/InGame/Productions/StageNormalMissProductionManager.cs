using InGame.Camera;
using InGame.Effect;
using InGame.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class StageNormalMissProductionManager : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _bodyObjName;
        [SerializeField] private float _waitingExplodeStartTime;
        [SerializeField] private float _waitingDeadPlayerPassiveTime;
        [SerializeField] private float _waitingRemainPlayerPassiveTime;
        [SerializeField] private float _waitingFadeStartTime;
        [SerializeField] private EffectControler _explosionEffectPrefab;
        [SerializeField] private CameraShakeData _explosionCamShakeData;
        [SerializeField] private CameraShake _cameraShake;
        [SerializeField] private Fade _fade;

        private GameObject _playerNorth;
        private GameObject _playerSouth;
        private bool _isStageMissed;

        private void Update()
        {
            if (_playerNorth == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(_playerNorthTag);
                if (obj != null)
                {
                    _playerNorth = obj;

                    PlayerBodyHealth h = obj.transform.GetComponentInChildren<PlayerBodyHealth>();
                    if (h != null) h.OnDead += OnPlayerNorthMiss;

                    Debug.Log($"Found: {_playerNorth.name}");
                }
            }

            if (_playerSouth == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(_playerSouthTag);
                if (obj != null)
                {
                    _playerSouth = obj;

                    PlayerBodyHealth h = obj.transform.GetComponentInChildren<PlayerBodyHealth>();
                    if (h != null) h.OnDead += OnPlayerSouthMiss;

                    Debug.Log($"Found: {_playerSouth.name}");
                }
            }
        }

        public void OnPlayerNorthMiss()
        {
            if (_isStageMissed) return;
            _isStageMissed = true;

            StartCoroutine(PlayerMissCoroutine(_playerNorth, _playerSouth));
        }

        public void OnPlayerSouthMiss()
        {
            if (_isStageMissed) return;
            _isStageMissed = true;

            StartCoroutine(PlayerMissCoroutine(_playerSouth, _playerNorth));
        }

        private IEnumerator PlayerMissCoroutine(GameObject deadPlayer, GameObject remainPlayer)
        {
            PlayerStageMiss(deadPlayer);
            PlayerStageMiss(remainPlayer);

            yield return WaitForEndOfVibrate(deadPlayer);

            yield return new WaitForSeconds(_waitingExplodeStartTime);

            GenerateExplodeEffect(deadPlayer);
            Debug.Log($"<color=blue>{deadPlayer} Exploded!</color>");

            yield return new WaitForSeconds(_waitingDeadPlayerPassiveTime);

            if(_cameraShake != null) _cameraShake.SetShake(_explosionCamShakeData);
            deadPlayer.gameObject.SetActive(false);

            yield return new WaitForSeconds(_waitingRemainPlayerPassiveTime);

            remainPlayer.gameObject.SetActive(false);

            yield return new WaitForSeconds(_waitingFadeStartTime);

            yield return WaitForEndOfFadeOut();

            ReloadCurrentScene();
        }

        private void PlayerStageMiss(GameObject player)
        {
            if(player == null) return;

            PlayerBodyMove move = player.GetComponentInChildren<PlayerBodyMove>();
            if (move != null) move.OnMissStage();

            PlayerLip lip = player.GetComponentInChildren<PlayerLip>();
            if (lip != null) lip.OnMissStage();

            PlayerBodyHealth health = player.GetComponentInChildren<PlayerBodyHealth>();
            if (health != null) health.SetIgnoreDamage(true);

            PlayerNormalGoalTrigger trigger = player.GetComponentInChildren<PlayerNormalGoalTrigger>();
            if (trigger != null) trigger.SetIgnoreGoal(true);
        }

        private IEnumerator WaitForEndOfVibrate(GameObject player)
        {
            if(player == null) yield break;

            PlayerBodyVibrater v = player.transform.GetComponentInChildren<PlayerBodyVibrater>();

            if(v != null) yield return v.VibrateCoroutine();
        }

        private void GenerateExplodeEffect(GameObject player)
        {
            if (player == null) return;

            Transform tr = player.transform.Find(_bodyObjName);

            if (tr != null) Instantiate(_explosionEffectPrefab).OnGenerated(tr.position);
        }

        private IEnumerator WaitForEndOfFadeOut()
        {
            if(_fade == null) yield break;
            yield return _fade.WaitForEndOfAnimationCoroutine();
        }

        private void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}