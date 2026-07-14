using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//GOD
//接続ui関連、スポーンもここにあるよ

public class GOD_ControllerConnectionUI : MonoBehaviour
{
    [Header("スロット表示")]
    [SerializeField] TextMeshProUGUI slot1StatusText;
    [SerializeField] TextMeshProUGUI slot2StatusText;

    [Header("キャラクター割り当て（スロット順）")]
    [SerializeField] GOD_CharacterData[] characterPerSlot = new GOD_CharacterData[2];

    [SerializeField] Key registerKey = Key.Enter;

    [Header("接続シーンのスポーン位置")]
    [SerializeField] Transform spawnPoint1P;
    [SerializeField] Transform spawnPoint2P;

    // スポーン済みキャラを保持
    private GameObject spawnedPlayer1;
    private GameObject spawnedPlayer2;

    void OnEnable()
    {
        GOD_ControllerConnectionManager.OnSlotConnected += HandleSlotConnected;
        UpdateUI();
    }

    void OnDisable()
    {
        GOD_ControllerConnectionManager.OnSlotConnected -= HandleSlotConnected;
    }

    void Update()
    {
        // --- シーン遷移（2台揃っているときのみ）---
        if (GOD_ControllerConnectionManager.AllSlotsReady())
        {
            return; // 登録処理はスキップ（遷移トリガーは削除）
        }

        // --- キーボード登録 ---
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard[registerKey].wasPressedThisFrame)
        {
            GOD_ControllerConnectionManager.TryRegisterDevice(keyboard, out _);
        }

        // --- Gamepad 登録 ---
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)   // ← Aボタンのみ
            {
                GOD_ControllerConnectionManager.TryRegisterDevice(gamepad, out _);
            }
        }
    }
    void HandleSlotConnected(int slot, InputDevice device)
    {
        if (GOD_PlayerData.Instance != null)
        {
            GOD_PlayerData.Instance.Slots[slot].Device = device;
            GOD_PlayerData.Instance.Slots[slot].CharacterPrefab =
                slot < characterPerSlot.Length && characterPerSlot[slot] != null
                ? characterPerSlot[slot].Prefab : null;
        }

        // キャラをスポーンして即操作可能に
        SpawnPlayerInConnectionScene(slot, device);

        UpdateUI();
    }

    void SpawnPlayerInConnectionScene(int slot, InputDevice device)
    {
        var slotData = GOD_PlayerData.Instance?.Slots[slot];
        if (slotData == null || slotData.CharacterPrefab == null)
        {
            Debug.LogWarning("slotData または CharacterPrefab が null！");
            return;
        }

        Transform spawnPoint = slot == 0 ? spawnPoint1P : spawnPoint2P;
        if (spawnPoint == null)
        {
            Debug.LogWarning("spawnPoint が null！");
            return;
        }

        // SwitchCurrentControlScheme の代わりに PlayerInput.Instantiate を使う
        string scheme = device is Gamepad ? "Gamepad" : "Keyboard";
        var go = PlayerInput.Instantiate(
            slotData.CharacterPrefab,
            controlScheme: scheme,
            pairWithDevice: device
        );

        go.transform.position = spawnPoint.position;
        go.transform.rotation = spawnPoint.rotation;

        if (slot == 0) spawnedPlayer1 = go.gameObject;
        else spawnedPlayer2 = go.gameObject;

        Debug.Log($"スポーン完了: {go.name}, scheme={scheme}, device={device}");
    }
    void UpdateUI()
    {
        var slots = GOD_ControllerConnectionManager.SlotDevices;

        bool p1 = slots[0] != null;
        bool p2 = slots[1] != null;

        slot1StatusText.text = p1 ? "" : "Press A";
        slot2StatusText.text = p2 ? "" : "Press A";

    }
}