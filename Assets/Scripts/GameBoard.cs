using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance => s_Instance;
    private static GameBoard s_Instance;

    public int Width;
    public int Height;
    public int tileSize;

    // Start is called before the first frame update
    void Start()
    {
        s_Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x * tileSize < Width; ++x)
        {
            Gizmos.DrawLine(transform.position + Vector3.right * x * tileSize, transform.position + Vector3.right * x * tileSize + Height * Vector3.forward);
        }

        Gizmos.DrawLine(transform.position + Vector3.right * Width, transform.position + Vector3.right * Width + Height * Vector3.forward);

        for (int y = 0; y * tileSize < Height; ++y)
        {
            Gizmos.DrawLine(transform.position + Vector3.forward * y * tileSize, transform.position + Vector3.forward * y * tileSize + Vector3.right * Width);
        }

        Gizmos.DrawLine(transform.position + Vector3.forward * Height, transform.position + Vector3.forward * Height + Vector3.right * Width);
    }
}
