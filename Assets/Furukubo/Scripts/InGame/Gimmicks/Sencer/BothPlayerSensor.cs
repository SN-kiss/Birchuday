using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    public class BothPlayerSensor : MonoBehaviour
    {
        [SerializeField] private string _playerNorthTag;
        [SerializeField] private string _playerSouthTag;
        [SerializeField] private string _playerBodyName;
        [SerializeField] private UnityEvent _onPassiveEvent;
        [SerializeField] private UnityEvent _onActiveEvent;

        private bool _isPlayerNorthStay;
        private bool _isPlayerSouthStay;
        private bool _isActive;

        private void FixedUpdate()
        {
            if(_isActive) _onActiveEvent?.Invoke();
            else _onPassiveEvent?.Invoke();
        }

        public void OnSencerColliderEnter(Collider2D col)
        {
            if (_isActive) return;

            if (col.gameObject.name == _playerBodyName)
            {
                if (col.transform.parent.tag == _playerNorthTag)
                {
                    _isPlayerNorthStay = true;
                    if (_isPlayerSouthStay) _isActive = true;
                }
                else if (col.transform.parent.tag == _playerSouthTag)
                {
                    _isPlayerSouthStay = true;
                    if (_isPlayerNorthStay) _isActive = true;
                }
            }
        }

        public void OnSencerColliderExit(Collider2D col)
        {
            if (_isActive) return;

            if (col.gameObject.name == _playerBodyName)
            {
                if (col.transform.parent.tag == _playerNorthTag)
                {
                    _isPlayerNorthStay = false;
                }
                else if (col.transform.parent.tag == _playerSouthTag)
                {
                    _isPlayerSouthStay = false;
                }
            }
        }
    }
}