using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public Pipe_SoundsPlay pipe;
    [Space]
    public int maxDistance = 20;
    public int identicalClipsCount = 1;
    [Space]
    public AudioSource soundPrefab;
    public int playersPoolSize = 30;

    AudioSource[] soundPlayers;

    private void Start()
    {
        soundPlayers = new AudioSource[playersPoolSize];
        for (int i = 0; i < playersPoolSize; i++)
        {
            soundPlayers[i] = Instantiate(soundPrefab, transform);
        }
        if (PlayerSinglton.IsGood)
        {
            transform.position = PlayerSinglton.PlayerPosition;
        }
    }

    private void Update()
    {
        if (!PlayerSinglton.IsGood) return;

        // TODO
        // [x] убери null клипы
        // [x] убери слишком далекие клипы
        // [x] разбей на группы по одинаковому клипу 
        // [ ] внутри группы сортировка по расстоянию от игрока
        // [ ] по порядку запускай из каждой следующий клип 
        var clipsNotNull = pipe.awaitingClips.Where(s => s.clipCollection != null);
        var clipsInRange = clipsNotNull.Where(s => (s.position - PlayerSinglton.PlayerPosition).sqrMagnitude < maxDistance * maxDistance);
        var identicalClips = clipsInRange.GroupBy(s => s.clipCollection);
        foreach (var sameClipsCollections in identicalClips)
        {
            int playCount = Mathf.Min(sameClipsCollections.Count(), identicalClipsCount);
            for (int i = 0; i < playCount; i++)
            {
                PlaySound(
                    sameClipsCollections.ElementAt(i).clipCollection.GetRandomClip(),
                    sameClipsCollections.ElementAt(i).position,
                    sameClipsCollections.ElementAt(i).parent);
            }
        }
        pipe.awaitingClips.Clear();
    }

    public void PlaySound(AudioClip clip, Vector3 position, Transform parent)
    {
        var readySource = soundPlayers.FirstOrDefault(s => !s.isPlaying);
        if (readySource == null)
        {
            Debug.LogWarning("!No Awailable Audio Source To Play Sound!");
            return;
        }
        readySource.transform.position = position;
        if (parent != null) readySource.transform.parent = parent;
        readySource.clip = clip;
        readySource.Play();
    }
}
