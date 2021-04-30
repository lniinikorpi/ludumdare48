using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource deathAudioSource;
    private void Update()
    {
        if (!deathAudioSource.isPlaying)
            Destroy(gameObject);
    }

    public void PlayDeathSound(AudioClip clip, float volume)
    {
        deathAudioSource.volume = volume;
        deathAudioSource.clip = clip;
        deathAudioSource.Play();
    }
}
