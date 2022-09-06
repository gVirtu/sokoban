using System;
using UnityEngine;

[System.Serializable]
public class LevelCompletionData : ISerializationCallbackReceiver
{
    public string key;
    public DateTime completedAt;
    public LevelCompletionData(string levelName)
    {
        key = levelName;
        completedAt = DateTime.UtcNow;
    }

    [SerializeField] private long completedAtTS;

    public void OnBeforeSerialize()
    {
        completedAtTS = completedAt.ToFileTimeUtc();
    }

    public void OnAfterDeserialize()
    {
        completedAt = DateTime.FromFileTimeUtc(completedAtTS);
    }
}
