using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    AudioSource audioSource;

    public void PlaySound(AudioClip sound)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(gameObject);
    }
}
