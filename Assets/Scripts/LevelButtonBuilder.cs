using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonBuilder : MonoBehaviour
{
    public GameObject levelButton;
    void Start()
    {
        TextAsset levelNamesAsset = Resources.Load<TextAsset>("levelNames");
        LevelNameList levelNames = JsonUtility.FromJson<LevelNameList>(levelNamesAsset.text);
        int progress = SaveDataManager.CountCompletedLevels();

        for(var i = 0; i < levelNames.names.Length; i++)
        {
            if (i > 0 && progress == 0) return;
            if (progress < i - 3) return;

            var levelName = levelNames.names[i];
            var button = Instantiate(levelButton, transform);

            LevelButtonHandler handler = button.GetComponent<LevelButtonHandler>();
            handler.levelFile = levelName;
            handler.index = i;

            if (!SaveDataManager.IsLevelComplete(levelName))
            {
                button.transform.Find("CompletionMarker").gameObject.SetActive(false);
            }
        }
    }
}
