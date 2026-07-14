using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GOD_ControllerConnectionManager : MonoBehaviour
{
    // 接続済みデバイス（スロット順）
    public static InputDevice[] SlotDevices = new InputDevice[2];

    public delegate void SlotChanged(int slot, InputDevice device);
    public static event SlotChanged OnSlotConnected;
    public static event SlotChanged OnSlotDisconnected;

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Disconnected || change == InputDeviceChange.Removed)
        {
            for (int i = 0; i < SlotDevices.Length; i++)
            {
                if (SlotDevices[i] == device)
                {
                    SlotDevices[i] = null;
                    OnSlotDisconnected?.Invoke(i, device);
                }
            }
        }
    }

    /// <summary>
    /// 指定デバイスを空きスロットに登録する（接続画面から呼ぶ）
    /// </summary>
    public static bool TryRegisterDevice(InputDevice device, out int slot)
    {
        // 既登録なら無視
        for (int i = 0; i < SlotDevices.Length; i++)
            if (SlotDevices[i] == device) { slot = i; return false; }

        // 空きスロットへ
        for (int i = 0; i < SlotDevices.Length; i++)
        {
            if (SlotDevices[i] == null)
            {
                SlotDevices[i] = device;
                OnSlotConnected?.Invoke(i, device);
                slot = i;
                return true;
            }
        }
        slot = -1;
        return false;
    }

    public static bool AllSlotsReady() =>
        SlotDevices[0] != null && SlotDevices[1] != null;

    public static void ResetAllSlots()
    {
        for (int i = 0; i < SlotDevices.Length; i++)
        {
            if (SlotDevices[i] != null)
            {
                var device = SlotDevices[i];
                SlotDevices[i] = null;
                OnSlotDisconnected?.Invoke(i, device);
            }
        }
    }
}