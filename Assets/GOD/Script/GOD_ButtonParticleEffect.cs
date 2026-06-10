using UnityEngine;
using UnityEngine.EventSystems;

public class GOD_ButtonParticleEffect : MonoBehaviour,
    ISelectHandler, ISubmitHandler
{
    [SerializeField] private Animator particleLeft;
    [SerializeField] private Animator particleRight;

    private void PlayParticles()
    {
        particleLeft.Play("Start_ParticleLeft", 0, 0f);  // 0f‚Å–ˆ‰ñæ“ª‚©‚çÄ¶
        if (particleRight != null)
            particleRight.Play("Start_ParticleRight", 0, 0f);
    }

    public void OnSelect(BaseEventData eventData) => PlayParticles();
    public void OnSubmit(BaseEventData eventData) => PlayParticles();
}