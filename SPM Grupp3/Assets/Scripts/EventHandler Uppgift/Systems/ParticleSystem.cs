using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleSystem : MonoBehaviour
{
    void Start()
    {
        // If any DieEvent is invoked, call the OnObjectExploded-method
        EventHandler.Instance.RegisterListener<DieEvent>(OnObjectExploded);
    }

    void OnObjectExploded(DieEvent eventInfo)
    {
        UnityEngine.ParticleSystem particleSystem = eventInfo.Invoker.GetComponent<UnityEngine.ParticleSystem>();
        GameObject particles = Instantiate(
            original: eventInfo.DeathParticles,
            position: eventInfo.Invoker.transform.position,
            rotation: eventInfo.Invoker.transform.rotation
            );
        Destroy(particles, particleSystem.main.startLifetime.constant);
    }
}

