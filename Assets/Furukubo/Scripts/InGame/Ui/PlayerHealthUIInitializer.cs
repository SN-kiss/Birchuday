using InGame.Player;
using UnityEngine;

namespace InGame.Ui
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerHealthUIInitializer : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private PlayerHeaithUi _playerNorthUi;
        [SerializeField] private PlayerHeaithUi _playerSouthUi;

        private bool _wasPlayerNorthFinded;
        private bool _wasPlayerSouthFinded;

        private void Update()
        {
            if (_playerNorthUi == null) return;
            if (_playerSouthUi == null) return;

            if (!_wasPlayerNorthFinded)
            {

                GameObject playerN = GameObject.FindGameObjectWithTag(_playerNorthTag);

                if (playerN != null)
                {
                    PlayerBodyHealth health = playerN.GetComponentInChildren<PlayerBodyHealth>();

                    if(health != null)
                    {
                        health.OnHealthChangedEvent += _playerNorthUi.SetAmounts;
                        health.InvokeOnHealthAmountChanged();
                        _wasPlayerNorthFinded = true;
                        Debug.Log("Found : Player North");
                    }
                }
            }

            if (!_wasPlayerSouthFinded)
            {
                GameObject playerS = GameObject.FindGameObjectWithTag(_playerSouthTag);

                if (playerS != null)
                {
                    PlayerBodyHealth health = playerS.GetComponentInChildren<PlayerBodyHealth>();

                    if (health != null)
                    {
                        health.OnHealthChangedEvent += _playerSouthUi.SetAmounts;
                        health.InvokeOnHealthAmountChanged();
                        _wasPlayerSouthFinded = true;
                        Debug.Log("Found : Player South");
                    }
                }
            }
        }
    }
}