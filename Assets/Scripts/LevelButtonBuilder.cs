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

        foreach (string level in levelNames.names)
        {
            var button = Instantiate(levelButton, transform);
            LevelButtonHandler handler = button.GetComponent<LevelButtonHandler>();
            handler.levelFile = level;

            if (!SaveDataManager.IsLevelComplete(level))
            {
                button.transform.Find("CompletionMarker").gameObject.SetActive(false);
            }
        }
    }
}
