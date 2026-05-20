using UnityEngine;
using UnityEngine.InputSystem;

public class GOD_PlayerData : MonoBehaviour
{
    public static GOD_PlayerData Instance { get; private set; }

    [System.Serializable]
    public class PlayerSlot
    {
        public InputDevice Device;
        public GameObject CharacterPrefab; // ‘I‘ðƒLƒƒƒ‰
    }

    public PlayerSlot[] Slots = new PlayerSlot[2]
    {
        new PlayerSlot(),
        new PlayerSlot()
    };

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
