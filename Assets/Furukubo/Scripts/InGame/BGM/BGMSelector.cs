using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public class BGMSelector : MonoBehaviour
{
    [SerializeField] private BGMType _type;

    private void Start()
    {
        if (BGMPlayer.Instance == null)
        {
            Debug.LogWarning("Instance of BGMPlayer was not found!");
        }
        else
        {
            BGMPlayer.Instance.SetAndPlayBGM(_type);
        }
    }
}