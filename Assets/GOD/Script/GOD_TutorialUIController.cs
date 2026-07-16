using UnityEngine;
using UnityEngine.InputSystem;

//GOD
//プレハブ側に置いたGOD_KissRelay.cs関係でチュートリアル終了のやつ

public class GOD_TutorialController : MonoBehaviour
{
    [Header("チュートリアルパネル")]
    [SerializeField] private GameObject ui_turn;
    [SerializeField] private GameObject ui_dash;
    [SerializeField] private GameObject ui_detach;

    [Header("入力しきい値")]
    [SerializeField, Range(0f, 1f)] private float stickThreshold = 0.3f;

    private enum Step
    {
        WaitingSpawn,   // 2人揃うのを待っている
        WaitingTurn,    // ui_turn 表示中：2人ともスティック入力待ち
        WaitingDash,    // ui_dash 表示中：2人ともAボタン入力待ち
        WaitingDetach,  // ui_detach 表示中：キス成立後、2人ともRT入力待ち
        Finished
    }

    private Step _step = Step.WaitingSpawn;

    // スロットごとのデバイス（0=PlayerA, 1=PlayerB）
    private readonly InputDevice[] _slotDevices = new InputDevice[2];

    private readonly bool[] _stepDone = new bool[2];

    private bool _isKissed;

    public static event System.Action OnTutorialCompleted;

    private void Awake()
    {
        SetActiveSafe(ui_turn, false);
        SetActiveSafe(ui_dash, false);
        SetActiveSafe(ui_detach, false);
    }

    private void OnEnable()
    {
        GOD_ControllerConnectionManager.OnSlotConnected += HandleSlotConnected;
        GOD_KissRelay.OnKiss += HandleKiss;
    }

    private void OnDisable()
    {
        GOD_ControllerConnectionManager.OnSlotConnected -= HandleSlotConnected;
        GOD_KissRelay.OnKiss -= HandleKiss;
    }

    private void HandleSlotConnected(int slot, InputDevice device)
    {
        if (slot < 0 || slot >= _slotDevices.Length) return;

        _slotDevices[slot] = device;

        // 2人とも揃ったらチュートリアル開始
        if (_step == Step.WaitingSpawn && _slotDevices[0] != null && _slotDevices[1] != null)
        {
            StartStep(Step.WaitingTurn, ui_turn);
        }
    }

    private void HandleKiss()
    {
        _isKissed = true;
    }

    private void Update()
    {
        switch (_step)
        {
            case Step.WaitingTurn:
                CheckBothPlayers(IsStickTilted, () => StartStep(Step.WaitingDash, ui_dash, ui_turn));
                break;

            case Step.WaitingDash:
                CheckBothPlayers(IsButtonSouthPressed, () => StartStep(Step.WaitingDetach, ui_detach, ui_dash));
                break;

            case Step.WaitingDetach:
                if (!_isKissed) return; // キス成立まではRTを受け付けない
                CheckBothPlayers(IsRightTriggerPressed, () => FinishStep(ui_detach));
                break;
        }
    }

    /// 各プレイヤーについて条件判定を行い、まだ達成していなければ記録する。
    /// 両方達成したら onBothDone を呼ぶ。
    private void CheckBothPlayers(System.Func<InputDevice, bool> condition, System.Action onBothDone)
    {
        for (int i = 0; i < 2; i++)
        {
            if (_stepDone[i]) continue;
            if (_slotDevices[i] == null) continue;

            if (condition(_slotDevices[i]))
            {
                _stepDone[i] = true;
            }
        }

        if (_stepDone[0] && _stepDone[1])
        {
            onBothDone?.Invoke();
        }
    }


    private bool IsStickTilted(InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            return gamepad.leftStick.ReadValue().sqrMagnitude >= stickThreshold * stickThreshold;
        }
        if (device is Keyboard keyboard)
        {
            return keyboard.wKey.isPressed || keyboard.aKey.isPressed ||
                   keyboard.sKey.isPressed || keyboard.dKey.isPressed ||
                   keyboard.upArrowKey.isPressed || keyboard.leftArrowKey.isPressed ||
                   keyboard.downArrowKey.isPressed || keyboard.rightArrowKey.isPressed;
        }
        return false;
    }

    private bool IsButtonSouthPressed(InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            return gamepad.buttonSouth.wasPressedThisFrame;
        }
        if (device is Keyboard keyboard)
        {
            return keyboard.spaceKey.wasPressedThisFrame;
        }
        return false;
    }

    private bool IsRightTriggerPressed(InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            return gamepad.rightTrigger.wasPressedThisFrame;
        }
        if (device is Keyboard keyboard)
        {
            return keyboard.zKey.wasPressedThisFrame;
        }
        return false;
    }

    private void StartStep(Step next, GameObject show, GameObject hide = null)
    {
        SetActiveSafe(hide, false);
        SetActiveSafe(show, true);

        _stepDone[0] = false;
        _stepDone[1] = false;

        _step = next;
    }

    private void FinishStep(GameObject hide)
    {
        SetActiveSafe(hide, false);
        _step = Step.Finished;

        OnTutorialCompleted?.Invoke();
    }

    private void SetActiveSafe(GameObject go, bool value)
    {
        if (go != null) go.SetActive(value);
    }
}