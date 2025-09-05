using Cysharp.Threading.Tasks;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] int deleteMiliseconds;

    ParticleSystem[] particleSystems;
    PlayerStatus status;

    float speed;
    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }
    private async void OnEnable()
    {
        speed = status == null ? 1 : status.ActionSpeedMultiplier;

        for(int i = 0; i < particleSystems.Length; i++)
        {
            var main = particleSystems[i].main;
            main.simulationSpeed = speed;
        }

        await delete();
    }
    async UniTask delete()
    {
        await UniTask.Delay((int)(deleteMiliseconds / speed));
        gameObject.SetActive(false);
    }
}
