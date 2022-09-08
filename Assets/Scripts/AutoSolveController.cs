using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSolveController : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    Queue<Direction> directionQueue = new Queue<Direction>();
    PlayerController controller;
    bool active = false;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }
    public bool IsActive()
    {
        return active;
    }

    public bool StartAutoSolve()
    {
        directionQueue.Clear();
        string solution = GUIUtility.systemCopyBuffer;

        foreach(char c in solution)
        {
            switch(char.ToUpper(c))
            {
                case 'U':
                    directionQueue.Enqueue(Direction.Up);
                    break;

                case 'D':
                    directionQueue.Enqueue(Direction.Down);
                    break;

                case 'L':
                    directionQueue.Enqueue(Direction.Left);
                    break;

                case 'R':
                    directionQueue.Enqueue(Direction.Right);
                    break;
            }
        }

        active = (directionQueue.Count > 0);
        return active;
    }

    public void NextStep()
    {
        if (directionQueue.Count == 0)
        {
            active = false;
            return;
        }

        Direction direction = directionQueue.Dequeue();
        controller.Move(GetDirectionVector(direction));
    }

    // Update is called once per frame
    Vector3 GetDirectionVector(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                return Vector3.back;

            case Direction.Down:
                return Vector3.forward;

            case Direction.Left:
                return Vector3.right;

            case Direction.Right:
                return Vector3.left;
        }

        return Vector3.zero;
    }
}
