using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CopyReplayButtonHandler : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    private void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void onCopyButtonClick()
	{
        string replay = ReplayManager.Instance.GetReplay();
        GUIUtility.systemCopyBuffer = replay;
        textMesh.text = "Copiado!";
	}
}
