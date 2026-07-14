using UnityEngine;
using UnityEngine.InputSystem;

//GOD
//操作説明ui関連

public class GOD_TutorialUIController : MonoBehaviour
{
    [Header("UIパネル（Canvas内の3つのパネルをアタッチ）")]
    [SerializeField] private GameObject uiTurn;
    [SerializeField] private GameObject uiDash;
    [SerializeField] private GameObject uiDetach;

    [Header("入力設定")]
    [Tooltip("左スティックをどれくらい倒したら『動かした』と判定するか（0〜1）")]
    [SerializeField] private float stickDeadzone = 0.3f;

    // 現在の進行状態
    private enum SequenceState
    {
        Waiting,      // Aボタン入力待ち（何も表示していない）
        TurnShown,    // ui_turn表示中。左スティック入力待ち
        DashShown,    // ui_dash表示中。Aボタン入力待ち
        DetachShown,  // ui_detach表示中。RT入力待ち
        Finished      // 一連の流れが完了した状態
    }

    private SequenceState currentState = SequenceState.Waiting;

    private void Start()
    {
        // シーンに入った瞬間は全パネル非表示＆状態リセット
        ResetSequence();
    }

    private void ResetSequence()
    {
        currentState = SequenceState.Waiting;
        SetPanelActive(uiTurn, false);
        SetPanelActive(uiDash, false);
        SetPanelActive(uiDetach, false);
    }

    private void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            // ゲームパッドが接続されていない場合は何もしない
            return;
        }

        switch (currentState)
        {
            case SequenceState.Waiting:
                // Aボタン（Xbox系パッドの buttonSouth）
                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    SetPanelActive(uiTurn, true);
                    currentState = SequenceState.TurnShown;
                }
                break;

            case SequenceState.TurnShown:
                // 左スティックをどの方向でもいいので一定以上倒したら反応
                if (gamepad.leftStick.ReadValue().magnitude > stickDeadzone)
                {
                    SetPanelActive(uiTurn, false);
                    SetPanelActive(uiDash, true);
                    currentState = SequenceState.DashShown;
                }
                break;

            case SequenceState.DashShown:
                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    SetPanelActive(uiDash, false);
                    SetPanelActive(uiDetach, true);
                    currentState = SequenceState.DetachShown;
                }
                break;

            case SequenceState.DetachShown:
                // RT（右トリガー）
                if (gamepad.rightTrigger.wasPressedThisFrame)
                {
                    SetPanelActive(uiDetach, false);
                    currentState = SequenceState.Finished;

                    OnSequenceFinished();
                }
                break;

            case SequenceState.Finished:
                // 完了後は何もしない（次にこのシーンを読み込み直した時に再度Start()から始まる）
                break;
        }
    }

  
    private void OnSequenceFinished()
    {
        Debug.Log("もう操作できるよねフフフ");
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}