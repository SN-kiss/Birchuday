using UnityEngine;

namespace InGame.Gimmick
{
    public class DammyGImmickInitializer : MonoBehaviour
    {
        [SerializeField] private PressurePlate _targetPressurePlate;
        [SerializeField] private Door _targetDoor;

        private void Awake()
        {
            if(_targetPressurePlate == null) return;
            if (_targetDoor == null) return;

            _targetPressurePlate.OnPressingEvent += () =>
            {
                if(_targetDoor != null) _targetDoor.OnCloseUpdate();
            };

            _targetPressurePlate.OnReleasingEvent += () =>
            {
                if (_targetDoor != null) _targetDoor.OnOpenUpdate();
            };
        }
    }
}