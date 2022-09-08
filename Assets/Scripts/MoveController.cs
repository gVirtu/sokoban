using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Transform moveTarget;
    public float moveSpeed;
    public GameObject boardObject;
    public bool canPush = false;
    public bool recordActions = false;

    public delegate void MoveEndDelegate();
    public event MoveEndDelegate OnMoveEnd;

    private bool moving;
    private Rigidbody rb;
    private GameBoard gameBoard;

    void Start()
    {
        moveTarget.parent = null;
        moving = false;
        rb = GetComponent<Rigidbody>();

        if (boardObject == null)
            boardObject = GameObject.FindWithTag("GameBoard");

        gameBoard = boardObject.GetComponent<GameBoard>();
    }

    public bool Move(Vector3 direction, int spaces)
    {
        float distance = gameBoard.tileSize * spaces;

        RaycastHit hitInfo;
        if (!moving)
        {
            direction += Vector3.up * 0.003f;
            bool hit = rb.SweepTest(direction, out hitInfo, distance, QueryTriggerInteraction.Ignore);
            bool moved = false;
            bool pushed = false;
            GameAction action = new GameAction();

            if (hit) {
                GameObject hitObject = hitInfo.collider.gameObject;

                if (canPush && hitObject.CompareTag("Pushable"))
                {
                    MoveController hitMoveController = hitObject.GetComponent<MoveController>();

                    if (hitMoveController != null && hitMoveController.Move(direction, spaces))
                    {
                        action.AddMovement(hitMoveController.gameObject, hitMoveController.moveTarget);
                        moved = true;
                        pushed = true;
                    }
                }
            }
            else
            {
                moved = true;
            }

            if (moved)
            {
                moveTarget.position += direction * distance;
                moving = true;

                action.AddMovement(gameObject, moveTarget);
                
                if (recordActions)
                {
                    ReplayManager.Instance.RecordMovement(direction, pushed);
                    action.SetReplayPosition(ReplayManager.Instance.MovementCount() - 1);
                    if (pushed) ActionStack.Instance.Push(action);
                }
            }
        }

        return moving;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
        {
            rb.position = Vector3.MoveTowards(rb.position, moveTarget.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(rb.position, moveTarget.position) <= .05f)
            {
                rb.position = moveTarget.position;
                moving = false;
                if (OnMoveEnd != null)
                    OnMoveEnd();
            }
        } else
        {
            moveTarget.position = rb.position;
        }
    }
}
