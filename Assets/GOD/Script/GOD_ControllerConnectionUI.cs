using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GOD_ControllerConnectionUI : MonoBehaviour
{
    [Header("スロット表示")]
    [SerializeField] TextMeshProUGUI slot1StatusText;  // 初期値 "－"
    [SerializeField] TextMeshProUGUI slot2StatusText;  // 初期値 "－"

    [Header("キャラクター画像")]
    [SerializeField] GameObject slot1Image;
    [SerializeField] GameObject slot2Image;

    [Header("次へボタン（2台揃ったら有効化）")]
    [SerializeField] TextMeshProUGUI NextText;

    // キーボードを「押した」とみなすキー
    [SerializeField] Key registerKey = Key.Space;

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
        if (GOD_ControllerConnectionManager.AllSlotsReady())
        {
            var kb = Keyboard.current;
            if (kb != null && (kb.spaceKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame))
            {
                GoToNextScene();
                return;
            }

            foreach (var device in GOD_ControllerConnectionManager.SlotDevices)
            {
                if (device is Gamepad gp && gp.buttonSouth.wasPressedThisFrame)
                {
                    GoToNextScene();
                    return;
                }
            }
            return;
        }

        // --- キーボード（Spaceキーで登録）---
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard[registerKey].wasPressedThisFrame)
        {
            GOD_ControllerConnectionManager.TryRegisterDevice(keyboard, out _);
        }

        // --- Gamepad（任意ボタンで登録）---
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame ||
                gamepad.buttonNorth.wasPressedThisFrame ||
                gamepad.buttonEast.wasPressedThisFrame ||
                gamepad.buttonWest.wasPressedThisFrame ||
                gamepad.startButton.wasPressedThisFrame)
            {
                GOD_ControllerConnectionManager.TryRegisterDevice(gamepad, out _);
            }
        }
    }

    void GoToNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GOD_MainTest");
    }

    void HandleSlotConnected(int slot, InputDevice device)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        var slots = GOD_ControllerConnectionManager.SlotDevices;

        bool p1 = slots[0] != null;
        bool p2 = slots[1] != null;

        slot1StatusText.text = slots[0] != null ? "OK" : "-";
        slot2StatusText.text = slots[1] != null ? "OK" : "-";

        if (slot1Image != null) slot1Image.SetActive(p1);
        if (slot2Image != null) slot2Image.SetActive(p2);

        if (NextText != null)
            NextText.gameObject.SetActive(GOD_ControllerConnectionManager.AllSlotsReady());
    }
}
