using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] ParticleSystem particleEffect;
    [SerializeField] Animation animationEffect;

    public void Init()
    {
        if (particleEffect != null)
        {
            particleEffect.gameObject.SetActive(false);
        }
        if (animationEffect != null)
        {
            animationEffect.gameObject.SetActive(false);
        }
    }
    public void PlayEffect()
    {
        if(particleEffect != null)
        {
            particleEffect.gameObject.SetActive(true);
            particleEffect.Simulate(0, true, true);
            particleEffect.Play();
        }
        if(animationEffect != null)
        {
            animationEffect.gameObject.SetActive(true);
            animationEffect.Play();
        }
    }
}
