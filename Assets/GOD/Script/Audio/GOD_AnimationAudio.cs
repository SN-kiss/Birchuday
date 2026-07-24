using UnityEngine;

public class GOD_AnimationAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips; // 複数の音を使い分けたい場合

    // 単一のAudioClipをアタッチして再生したい場合
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // インデックス指定で複数の音を切り替えたい場合
    public void PlaySoundByIndex(int index)
    {
        if (index >= 0 && index < clips.Length)
            audioSource.PlayOneShot(clips[index]);
    }

    // 単純に1つの音を毎回鳴らす場合(引数なし)
    public void PlayFootstep()
    {
        audioSource.PlayOneShot(clips[0]);
    }
}