using UnityEngine;

public class GOD_AnimationAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Animation Event‚©‚ç‚±‚ÌŠÖ”‚ğŒÄ‚Ô
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}