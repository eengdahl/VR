using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVImpactSound : MonoBehaviour
{
    public List<AudioClip> soundEffect;

    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    public float volume = 1f;
    public bool onlyPlayOnce = true;

    private AudioSource audioSource;

    void OnCollisionEnter(Collision collision)
    {
        if (soundEffect == null)
        {
            return;
        }

        bool hasPlayed = (audioSource != null);
        if (audioSource == null)
        {
            if (!GetComponent<AudioSource>())
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                audioSource = GetComponent<AudioSource>();
            }
            audioSource.clip = soundEffect[Random.Range(1, soundEffect.Count - 1)];
        }

        if (!hasPlayed || !onlyPlayOnce)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
}
