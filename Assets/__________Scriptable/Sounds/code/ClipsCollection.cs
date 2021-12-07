using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCollection", menuName = "MySounds/Collection")]
public class ClipsCollection : ScriptableObject
{
    public AudioClip[] clips;

    public AudioClip GetRandomClip()
    {
        int clipId = Random.Range(0, clips.Length);
        return clips[clipId];
    }
}
