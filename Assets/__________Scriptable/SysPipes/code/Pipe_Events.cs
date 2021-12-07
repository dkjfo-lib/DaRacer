using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Pipe_Events", menuName = "Pipes/Pipe_Events")]
public class Pipe_Events : ScriptableObject
{
    public List<EventData> events;

    public void SendEvent(string eventTag)
    {
        EventData data = events.FirstOrDefault((s) => s.tag == eventTag);
        if (data == null)
        {
            events.Add(new EventData
            {
                tag = eventTag,
                count = 1
            });
        }
        else
        {
            data.count += 1;
        }
    }

    public int GetEventCount(string eventTag)
    {
        int count;
        EventData data = events.FirstOrDefault((s) => s.tag == eventTag);
        if (data == null)
        {
            count = 0;
        }
        else
        {
            count = data.count;
        }
        return count;
    }
}

[System.Serializable]
public class EventData
{
    public string tag;
    public int count;
}