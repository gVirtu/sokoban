using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance { get; private set; }

    private List<char> movements = new List<char>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void RecordMovement(Vector3 direction, bool pushed)
    {
        char movement = MapMovementToChar(direction);

        if (pushed) movement = char.ToUpper(movement);

        movements.Add(movement);
    }

    public void TrimToIndex(int index)
    {
        movements.RemoveRange(index, movements.Count - index);
    }

    public int MovementCount()
    {
        return movements.Count;
    }

    char MapMovementToChar(Vector3 direction)
    {
        if (direction.x < -0.5f) return 'r';
        if (direction.x > 0.5f) return 'l';
        if (direction.z < -0.5f) return 'u';
        if (direction.z > 0.5f) return 'd';
        return ' ';
    }

    // Update is called once per frame
    public string GetReplay()
    {
        return string.Join("", movements);
    }
}
