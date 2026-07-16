using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//GOD
//接続ui関連、スポーンもここにあるよ
public class GOD_ControllerConnectionUI : MonoBehaviour
{
    [Header("スロット1：Press A画像／接続完了画像")]
    [SerializeField] GameObject slot1PressAImage;
    [SerializeField] GameObject slot1ConnectedImage;
    [Header("スロット2：Press A画像／接続完了画像")]
    [SerializeField] GameObject slot2PressAImage;
    [SerializeField] GameObject slot2ConnectedImage;
    [Header("接続完了画像の表示時間")]
    [SerializeField] float connectedDisplayDuration = 2f;

    [Header("キャラクター割り当て（スロット順）")]
    [SerializeField] GOD_CharacterData[] characterPerSlot = new GOD_CharacterData[2];
    [SerializeField] Key registerKey = Key.Enter;
    [Header("接続シーンのスポーン位置")]
    [SerializeField] Transform spawnPoint1P;
    [SerializeField] Transform spawnPoint2P;
    // スポーン済みキャラを保持
    private GameObject spawnedPlayer1;
    private GameObject spawnedPlayer2;

    // スロットごとの演出コルーチン管理
    private Coroutine slot1EffectCoroutine;
    private Coroutine slot2EffectCoroutine;

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

        // 接続完了画像を一時表示（Press A画像は非表示に）
        PlayConnectedEffect(slot);
    }

    void PlayConnectedEffect(int slot)
    {
        GameObject pressA = slot == 0 ? slot1PressAImage : slot2PressAImage;
        GameObject connected = slot == 0 ? slot1ConnectedImage : slot2ConnectedImage;

        if (pressA != null) pressA.SetActive(false);

        if (connected == null) return;

        if (slot == 0)
        {
            if (slot1EffectCoroutine != null) StopCoroutine(slot1EffectCoroutine);
            slot1EffectCoroutine = StartCoroutine(ShowConnectedImageRoutine(connected, slot));
        }
        else
        {
            if (slot2EffectCoroutine != null) StopCoroutine(slot2EffectCoroutine);
            slot2EffectCoroutine = StartCoroutine(ShowConnectedImageRoutine(connected, slot));
        }
    }

    IEnumerator ShowConnectedImageRoutine(GameObject connectedImage, int slot)
    {
        connectedImage.SetActive(true);

        yield return new WaitForSeconds(connectedDisplayDuration);

        connectedImage.SetActive(false);

        if (slot == 0) slot1EffectCoroutine = null;
        else slot2EffectCoroutine = null;
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

        // 未接続なら Press A 画像を表示、接続済みなら非表示
        // （接続演出中は HandleSlotConnected 側で制御するのでここでは触らない）
        if (slot1PressAImage != null && slot1EffectCoroutine == null)
        {
            slot1PressAImage.SetActive(!p1);
        }
        if (slot2PressAImage != null && slot2EffectCoroutine == null)
        {
            slot2PressAImage.SetActive(!p2);
        }

        // 接続完了画像は演出中以外は非表示（起動時などの初期化用）
        if (slot1ConnectedImage != null && slot1EffectCoroutine == null && !p1)
        {
            slot1ConnectedImage.SetActive(false);
        }
        if (slot2ConnectedImage != null && slot2EffectCoroutine == null && !p2)
        {
            slot2ConnectedImage.SetActive(false);
        }
    }
}