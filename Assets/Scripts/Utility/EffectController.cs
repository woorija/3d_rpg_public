using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] int deleteMiliseconds;

    ParticleSystem[] particleSystems;
    float[] baseSimulationSpeeds;

    PlayerStatus status;
    float speedMultiplier;

    private void Awake()
    {
        status = GetComponentInParent<PlayerStatus>();
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        baseSimulationSpeeds = new float[particleSystems.Length];

        for(int i = 0; i < particleSystems.Length; i++)
        {
            baseSimulationSpeeds[i] = particleSystems[i].main.simulationSpeed;
        }
    }
    private void OnEnable()
    {
        EffectInitAsync().Forget();
    }
    async UniTaskVoid EffectInitAsync()
    {
        speedMultiplier = status == null ? 1 : status.ActionSpeedMultiplier;

        for (int i = 0; i < particleSystems.Length; i++)
        {
            var main = particleSystems[i].main;
            main.simulationSpeed = baseSimulationSpeeds[i] * speedMultiplier;
        }

        try
        {
            await UniTask.Delay((int)(deleteMiliseconds / speedMultiplier), cancellationToken: this.GetCancellationTokenOnDestroy());

            if (!this)
            {
                return;
            }

            gameObject.SetActive(false);
        }
        catch (OperationCanceledException)
        {
        }
    }
}
