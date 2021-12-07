using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    public ClipsCollection currentPlaylist;
    public bool play = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySongs(currentPlaylist);
    }

    public void PlaySongs(ClipsCollection music)
    {
        currentPlaylist = music;
        StartCoroutine(PlayMusic());
    }

    IEnumerator PlayMusic()
    {
        play = true;
        while (play)
        {
            audioSource.clip = currentPlaylist.GetRandomClip();
            audioSource.Play();
            yield return new WaitWhile(() => play && audioSource.isPlaying);
        }
    }

    public void ChangePlaylist(ClipsCollection newMusic)
    {
        StartCoroutine(ChangePlaylistIE(newMusic));
    }
    IEnumerator ChangePlaylistIE(ClipsCollection newMusic)
    {
        currentPlaylist = newMusic;
        play = false;
        // waiting for player to turn self off
        yield return new WaitForSeconds(.25f);
        StartCoroutine(PlayMusic());
    }
}
