using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    string selectedLevel;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedLevel(string level)
    {
        selectedLevel = level;
    }

    public string GetSelectedLevel()
    {
        return selectedLevel;
    }
}
