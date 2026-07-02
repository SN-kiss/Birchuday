using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public class BGMSelector : MonoBehaviour
{
    [SerializeField] private BGMType _type;

    private void Start()
    {
        if (BGMPlayer.Instance != null) BGMPlayer.Instance.SetAndPlayBGM(_type);
    }
}