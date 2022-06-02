using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : MonoBehaviour
{
    private PlayerInput playerInput;
    private TankState state;
    private InputAction fireAction;
    [SerializeField] private float damage;
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private AudioClip flameThrowerSound;

    public static float FireDamage;


    void Start()
    {
        state = GetComponentInParent<TankState>();
        playerInput = state.PlayerInput;
        fireAction = playerInput.actions["Shoot"];
        FireDamage = damage;
    }

	// Update is called once per frame
	private void LateUpdate()
	{
        if(fireAction.IsPressed())
        {
            if (!SoundSystem.instance.audioSource.isPlaying)
            {
                EventHandler.InvokeEvent(new PlaySoundEvent("Player Fire Shooting", flameThrowerSound));
            }
            
            fireParticles.Play();
        }
        else
        {
            fireParticles.Stop();
            SoundSystem.instance.StopAudio();
        }
    }
}
