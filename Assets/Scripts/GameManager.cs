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
    }
}
