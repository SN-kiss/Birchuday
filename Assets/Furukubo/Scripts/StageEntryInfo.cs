using UnityEngine;

public class StageEntryInfo : MonoBehaviour
{
    public static StageEntryInfo Instance { get; private set; }

    public StageEntryState State { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SetEntryState(StageEntryState.First);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void SetEntryState(StageEntryState state) => State = state;
}

public enum StageEntryState
{
    First,
    Clear,
    Miss
}