using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonHandler : MonoBehaviour
{
	public string levelFile;

	public void onLevelButtonClick()
	{
		GameManager.Instance.SetSelectedLevel(levelFile);

		SceneManager.LoadScene("Level");
	}
}
