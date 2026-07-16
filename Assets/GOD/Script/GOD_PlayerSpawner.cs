using UnityEngine;
using UnityEngine.InputSystem;
public class GOD_PlayerSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint1P;
    [SerializeField] Transform spawnPoint2P;
    void Start()
    {
        var data = GOD_PlayerData.Instance;
        if (data == null) { Debug.LogWarning("GOD_PlayerData ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ"); return; }
        SpawnPlayer(0, spawnPoint1P);
        SpawnPlayer(1, spawnPoint2P);
    }
    void SpawnPlayer(int slot, Transform spawnPoint)
    {
        var slotData = GOD_PlayerData.Instance.Slots[slot];
        if (slotData.CharacterPrefab == null || slotData.Device == null) return;
        string scheme = slotData.Device is Gamepad ? "Gamepad" : "Keyboard";
        var go = PlayerInput.Instantiate(
            slotData.CharacterPrefab,
            controlScheme: scheme,
            pairWithDevice: slotData.Device
        );
        go.transform.position = spawnPoint.position;
        go.transform.rotation = spawnPoint.rotation;
    }
}