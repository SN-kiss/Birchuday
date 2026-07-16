using InGame.Gimmick;
using UnityEngine;

//GOD
//接続画面のドアをGOD_TutorialController.csでの処理後開くようにするコード

public class GOD_TutorialDoorOpener : MonoBehaviour
{
    [SerializeField] private Door _targetDoor;

    private bool _shouldOpen;

    private void OnEnable()
    {
        GOD_TutorialController.OnTutorialCompleted += HandleTutorialCompleted;
    }

    private void OnDisable()
    {
        GOD_TutorialController.OnTutorialCompleted -= HandleTutorialCompleted;
    }

    private void HandleTutorialCompleted()
    {
        _shouldOpen = true;
    }

    private void FixedUpdate()
    {
        if (_shouldOpen && _targetDoor != null)
        {
            _targetDoor.OnOpenUpdate();
        }
    }
}