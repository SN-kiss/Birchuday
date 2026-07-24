using UnityEngine;
using UnityEngine.Audio;

public class GOD_SEPlayer : MonoBehaviour
{
    public static GOD_SEPlayer Instance { get; private set; }

    [SerializeField] private AudioMixerGroup _seMixerGroup;
    [SerializeField] private AudioClip _selectClip;
    [SerializeField] private AudioClip _clickClip;

    private AudioSource _seSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _seSource = gameObject.AddComponent<AudioSource>();
        _seSource.outputAudioMixerGroup = _seMixerGroup;
        _seSource.playOnAwake = false;
    }

    public void PlaySelect() => PlayOneShot(_selectClip);
    public void PlayClick() => PlayOneShot(_clickClip);

    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        _seSource.PlayOneShot(clip);
    }
}