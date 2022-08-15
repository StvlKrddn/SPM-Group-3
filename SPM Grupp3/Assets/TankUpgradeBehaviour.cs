using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUpgradeBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem effect;

    private void Start()
    {
        effect = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    public void PlayUpgradeEffect()
    {
        effect.Play();
    }

    public void StopUpgradeEffect()
    {
        effect.Stop();
    }
}
