using UnityEngine;
using UnityEngine.InputSystem;

public class GOD_PlayerSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint1P;
    [SerializeField] Transform spawnPoint2P;

    void Start()
    {
        var data = GOD_PlayerData.Instance;
        if (data == null) { Debug.LogWarning("GOD_PlayerData が見つかりません"); return; }

        SpawnPlayer(0, spawnPoint1P);
        SpawnPlayer(1, spawnPoint2P);
    }

    void SpawnPlayer(int slot, Transform spawnPoint)
    {
        var slotData = GOD_PlayerData.Instance.Slots[slot];
        if (slotData.CharacterPrefab == null || slotData.Device == null) return;

        var go = Instantiate(slotData.CharacterPrefab, spawnPoint.position, spawnPoint.rotation);

        var playerInput = go.GetComponent<PlayerInput>();
        if (playerInput == null) return;

        // デバイスに応じた ControlScheme 名を決定
        string scheme = slotData.Device is Gamepad ? "Gamepad" : "Keyboard";

        // このキャラは指定デバイスのみ受け付けるように紐づける
        playerInput.SwitchCurrentControlScheme(scheme, slotData.Device);
    }
}