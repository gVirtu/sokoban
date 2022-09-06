using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLevelButtonHandler : MonoBehaviour
{
    public void onWinButtonClick()
	{
		SaveDataManager.SaveCompleteLevel(GameManager.Instance.GetSelectedLevel());
		SceneManager.LoadScene("LevelSelect");
	}
}
