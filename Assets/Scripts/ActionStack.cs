using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement
{
    private GameObject gameObject;
    private Vector3 from;
    private Vector3 to;
    private Quaternion fromRotation;

    public Movement(GameObject gameObject, Transform from, Transform to) {
        this.gameObject = gameObject;
        this.from = from.position;
        this.fromRotation = from.rotation;
        this.to = to.position;
    }

    public GameObject GetGameObject() => gameObject;
    public Vector3 GetFrom() => from;
    public Vector3 GetTo() => to;
    public Quaternion GetFromRotation() => fromRotation;
}

public class GameAction
{
    private List<Movement> movements = new List<Movement>();

    public GameAction()
    {
    }

    public void AddMovement(GameObject obj, Transform target)
    {
        Movement movement = new Movement(obj, obj.transform, target);
        movements.Add(movement);
    }

    public void Rollback()
    {
        foreach (Movement movement in movements)
        {
            GameObject gameObject = movement.GetGameObject();
            Vector3 targetPosition = movement.GetFrom();
            Quaternion targetRotation = movement.GetFromRotation();

            gameObject.transform.SetPositionAndRotation(targetPosition, targetRotation);
        }
    }
}

public class ActionStack : MonoBehaviour
{
    public static ActionStack Instance { get; private set; }

    public TextMeshProUGUI actionsText;

    private Stack<GameAction> actions = new Stack<GameAction>();

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

    public void Push(GameAction action)
    {
        actions.Push(action);
        RefreshActionsText();
    }

    public void Pop()
    {
        if (actions.Count > 0)
        {
            print("Popping action");
            GameAction action = actions.Pop();
            action.Rollback();
            RefreshActionsText();
        }
    }

    private void RefreshActionsText()
    {
        actionsText.SetText("Ações: " + actions.Count.ToString());
    }
}
