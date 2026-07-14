using UnityEngine;
using UnityEngine.InputSystem;

public class GOD_ResetControllerLinks : MonoBehaviour
{
    void Awake()
    {
        // シーン内に残っている PlayerInput を全部ペアリング解除して破棄
        var players = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
        foreach (var p in players)
        {
            p.user.UnpairDevicesAndRemoveUser();
            Destroy(p.gameObject);
        }

        // マネージャー側のスロット情報をクリア
        GOD_ControllerConnectionManager.ResetAllSlots();

        // PlayerDataのスロットもクリア(存在する場合)
        if (GOD_PlayerData.Instance != null)
        {
            foreach (var slot in GOD_PlayerData.Instance.Slots)
            {
                slot.Device = null;
                slot.CharacterPrefab = null;
            }
        }
    }
}