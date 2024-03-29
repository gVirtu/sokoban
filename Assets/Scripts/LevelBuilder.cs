using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class LevelBuilder : MonoBehaviour
{
    public string levelPath;
    public float tileSize = 2f;
    public float zoomDistanceFactor = 0.02f;
    public CinemachineVirtualCameraBase cinemachineCam;
    public Vector3 baseCameraFollow = new Vector3(0, 20, 15);
    public GameObject objWall;
    public GameObject objPlayer;
    public GameObject objBox;
    public GameObject objZone;
    public GameObject objFloor;

    private Vector3 originalFollow;
    private List<GameObject> levelObjects;

    void Start()
    {
        TextAsset levelAsset = (TextAsset)Resources.Load("Levels/" + GameManager.Instance.GetSelectedLevel(), typeof(TextAsset));

        string content = levelAsset.text;
        string[] lines = content.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        int rows = lines.Length;
        int cols = lines[rows - 1].Length;

        levelObjects = new List<GameObject>();

        BuildLevel(lines, rows, cols);
        ZoomCamera(Math.Max(rows, cols));
        FollowLevelCenter(rows, cols);
    }


    void BuildLevel(string[] tiles, int rows, int cols)
    {
        int i, j;
        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < cols; j++)
            {
                CreateFloor(tiles[i][j], i, j);
                CreateTile(tiles[i][j], i, j);
            }
        }
    }

    void ZoomCamera(int dimension)
    {
        var transposer = (cinemachineCam as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineTransposer>();
        originalFollow = transposer.m_FollowOffset;
        transposer.m_FollowOffset = baseCameraFollow * dimension * zoomDistanceFactor;
    }

    void FollowLevelCenter(int rows, int columns)
    {
        var instance = Instantiate(new GameObject(), new Vector3(-(columns - 1) * 0.5f * tileSize, 0f, (rows - 1) * 0.5f * tileSize), Quaternion.identity);

        cinemachineCam.LookAt = instance.transform;
        cinemachineCam.Follow = instance.transform;
    }

    void CreateFloor(char tile, int row, int column)
    {
        switch (tile)
        {
            case '@':
            case '$':
            case ' ':
                levelObjects.Add(Instantiate(objFloor, new Vector3(-column * tileSize, 0f, row * tileSize), Quaternion.identity));
                break;

            case '.':
            case '*':
            case '+':
                levelObjects.Add(Instantiate(objZone, new Vector3(-column * tileSize, 0f, row * tileSize), Quaternion.identity));
                break;
        }
    }

    void CreateTile(char tile, int row, int column)
    {
        Vector3 position = new Vector3(-column * tileSize, 1f, row * tileSize);
        Quaternion rotation = Quaternion.identity;
        GameObject obj = null;
        Action<GameObject> callback = null;

        switch (tile)
        {
            case '#':
                obj = objWall;
                break;

            case '@':
            case '+':
                obj = objPlayer;
                callback = onCreatePlayer;
                position += Vector3.up * 0.5f;
                break;

            case '$':
            case '*':
                obj = objBox;
                break;
        }

        if (obj != null)
        {
            GameObject instance = Instantiate(obj, position, rotation);
            levelObjects.Add(instance);

            if (callback != null)
            {
                callback.Invoke(instance);
            }
        }
    }

    void onCreatePlayer(GameObject instance)
    {
        //cinemachineCam.LookAt = instance.transform;
        //cinemachineCam.Follow = instance.transform;

        PlayerController playerController = instance.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.cinemachineCam = cinemachineCam;
            playerController.mainCamera = GameObject.FindWithTag("MainCamera").transform;
        }
    }

    void OnDestroy()
    {
        foreach (var instance in levelObjects)
        {
            Destroy(instance);
        }
    }
}
