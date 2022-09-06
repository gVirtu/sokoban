using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData : ISerializationCallbackReceiver
{
    public Dictionary<string, LevelCompletionData> levelCompletions = new Dictionary<string, LevelCompletionData>();

    [SerializeField] private LevelCompletionData[] completions;

    public void OnBeforeSerialize()
    {
        List<LevelCompletionData> completionList = new List<LevelCompletionData>(); 
        foreach (var completion in levelCompletions)
        {
            completionList.Add(completion.Value);
        }

        completions = completionList.ToArray();
    }

    public void OnAfterDeserialize()
    {
        levelCompletions = new Dictionary<string, LevelCompletionData>();

        foreach(var completion in completions)
        {
            levelCompletions.Add(completion.key, completion);
        }
    }
}
