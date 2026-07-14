using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 特定のシーン専用のUI表示シーケンス制御スクリプト。
/// このスクリプトは「対象のシーンに存在するGameObject」にアタッチしてください。
/// シーンが読み込まれるたびに Start() が呼ばれて状態がリセットされるため、
/// 「このシーンに戻ってくると再びこの一連の流れが発動する」動作は自動的に満たされます。
/// （他のシーンには絶対にこのスクリプトを置かないでください＝それだけで「このシーンのみで動く」を実現できます）
/// </summary>
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

    /// <summary>
    /// 一連の流れ（ui_turn→ui_dash→ui_detach→非表示）が完了した時に呼ばれる。
    /// ここに「今から作ろうとしているスクリプト」の処理を呼び出してください。
    /// 例：GetComponent<次のスクリプト>().enabled = true; など
    /// </summary>
    private void OnSequenceFinished()
    {
        // TODO: ここに完了後の処理を書く
        Debug.Log("UIシーケンスが完了しました。");
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}