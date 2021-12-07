using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXPlaylist", menuName = "Pipes/SFXPlaylist")]
public class Pipe_SoundsPlay : ScriptableObject
{
    public List<PlayClipData> awaitingClips = new List<PlayClipData>();

    public void AddClip(PlayClipData clipData)
    {
        awaitingClips.Add(clipData);
    }
}

[System.Serializable]
public struct PlayClipData
{
    public ClipsCollection clipCollection;
    public Vector3 position;
    public Transform parent;

    public PlayClipData(ClipsCollection clipCollection, Vector3 position, Transform parent = null)
    {
        this.clipCollection = clipCollection;
        this.position = position;
        this.parent = parent;
    }
}