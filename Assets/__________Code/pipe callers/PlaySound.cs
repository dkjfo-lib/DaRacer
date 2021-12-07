using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public Pipe_SoundsPlay Pipe_SoundsPlay;
    public ClipsCollection sound;
    public bool onStart = true;
    public bool onDestroy = true;

    void Start()
    {
        if (onStart)
            Pipe_SoundsPlay.AddClip(new PlayClipData(sound, transform.position));
    }

    private void OnDestroy()
    {
        if (onDestroy)
            Pipe_SoundsPlay.AddClip(new PlayClipData(sound, transform.position));
    }
}
