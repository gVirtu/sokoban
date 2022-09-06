#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class PreBuildLevelDiscovery : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        bool newLevel = false;
        foreach (string str in importedAssets)
        {
            if (str.StartsWith("Assets/Resources/Levels"))
            {
                newLevel = true;
                break;
            }
        }

        if (!newLevel)
        {
            foreach (string str in deletedAssets)
            {
                if (str.StartsWith("Assets/Resources/Levels"))
                {
                    newLevel = true;
                    break;
                }
            }
        }

        if (!newLevel) return;

        string resourcesPath = Application.dataPath + "/Resources/Levels";

        string[] fileNames = Directory.GetFiles(resourcesPath)
            .Where(x => Path.GetExtension(x) == ".txt").Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();

        LevelNameList levelNames = new LevelNameList(fileNames);

        Debug.Log("Found " + levelNames.names.Length + " levels");

        string fileInfoJson = JsonUtility.ToJson(levelNames);

        File.WriteAllText(Application.dataPath + "/Resources/levelNames.txt", fileInfoJson);
        AssetDatabase.Refresh();
    }
}

#endif