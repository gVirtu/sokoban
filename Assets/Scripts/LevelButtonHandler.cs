using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonHandler : MonoBehaviour
{
	public string levelFile;

    private void Start()
    {
		string levelCaption = (transform.GetSiblingIndex() + 1).ToString();

		TextMeshProUGUI tmp = transform.GetComponentInChildren<TextMeshProUGUI>();
		tmp.text = levelCaption;
    }

    public void onLevelButtonClick()
	{
		GameManager.Instance.SetSelectedLevel(levelFile);

		StartLevelButtonHandler.StartButton.interactable = true;

		PreviewPanel.Instance.RefreshPreview();
	}
}
