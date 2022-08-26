using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevelButtonHandler : MonoBehaviour
{
	public static Button StartButton;

    private void Awake()
    {
        if (StartButton != null)
        {
            Destroy(gameObject);
            return;
        }

        StartButton = GetComponent<Button>();
    }

    public void onStartButtonClick()
	{
		if (GameManager.Instance.GetSelectedLevel() != null)
        {
			SceneManager.LoadScene("Level");
		}
	}
}
