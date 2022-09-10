using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioClip WinBGM;

    [System.Serializable]
    public class PrefabLibrary
    {
        public GameObject lightBeam;
    }

    public PrefabLibrary Prefabs;

    string selectedLevel;
    int selectedLevelIndex;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SaveDataManager.LoadGame();
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedLevel(string level, int index)
    {
        selectedLevel = level;
        selectedLevelIndex = index;
    }

    public string GetSelectedLevel()
    {
        return selectedLevel;
    }

    public int GetSelectedLevelIndex()
    {
        return selectedLevelIndex;
    }

    public void CheckVictory()
    {
        foreach(var pushable in GameObject.FindGameObjectsWithTag("Pushable"))
        {
            var controller = pushable.GetComponent<PushBoxController>();

            if (controller != null)
            {
                if (!controller.IsLit()) return;
            }
        }

        WinLevel();
    }

    public void WinLevel()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            controller.HandleLevelVictory();
        }

        var bgm = GameObject.FindGameObjectWithTag("BGM");
        
        if (bgm != null)
        {
            var audioSource = bgm.GetComponent<AudioSource>();
            audioSource.Stop();
            audioSource.clip = WinBGM;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}
