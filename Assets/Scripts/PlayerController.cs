using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform moveTarget;
    public Transform mainCamera;
    public GameObject boardObject;
    public float moveSpeed;
    public float fallingThreshold = -0.5f;

    private enum State
    {
        Idle,
        Moving,
        Falling
    }

    private State state;
    private Vector2 movementVector;
    private GameBoard gameBoard;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        moveTarget.parent = null;
        state = State.Idle;
        gameBoard = boardObject.GetComponent<GameBoard>();
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
    }

    void HandleMovement() 
    {
        Vector3 direction = Vector3.zero;
        float distance = gameBoard.tileSize;
        bool moved = false;

        Vector3 target = moveTarget.position;

        if (Mathf.Abs(movementVector.x) > 0.5)
        {
            direction = Vector3.right * Mathf.Sign(movementVector.x);
            moved = true;
        }
        else if (Mathf.Abs(movementVector.y) > 0.5)
        {
            direction = Vector3.forward * Mathf.Sign(movementVector.y);
            moved = true;
        }

        float cameraDirection = mainCamera.rotation.eulerAngles.y;
        float snappedCameraDirection = (float)(((int)90f * Mathf.Round(cameraDirection / 90f)) % 360);
        direction = Quaternion.Euler(0, snappedCameraDirection, 0) * direction;

        RaycastHit hit;
        if (moved && !rb.SweepTest(direction, out hit, distance))
        {
            state = State.Moving;
            target += direction * distance;
            moveTarget.position = target;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                moveTarget.position = transform.position;
                HandleMovement();
                if (rb.velocity.y < fallingThreshold)
                {
                    state = State.Falling;
                }
                break;

            case State.Moving:
                transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, moveTarget.position) <= .05f) {
                    transform.position = moveTarget.position;
                    state = State.Idle;
                }
                break;

            case State.Falling:
                if (rb.velocity.y > fallingThreshold)
                {
                    state = State.Idle;
                }
                break;
        }
    }
}
