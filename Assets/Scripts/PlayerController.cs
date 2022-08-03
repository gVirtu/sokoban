using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform mainCamera;
    public GameObject model;
    public float fallingThreshold = -0.5f;

    private enum State
    {
        Idle,
        Moving,
        Falling
    }

    private State state;
    private Vector2 movementVector;
    private Vector3 lastMovementDirection = Vector3.forward;
    private Rigidbody rb;
    private MoveController moveController;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        moveController = GetComponent<MoveController>();
        moveController.OnMoveEnd += HandleMoveEnd;
    }

    void OnDisable()
    {
        moveController.OnMoveEnd -= HandleMoveEnd;
    }

    void HandleMoveEnd()
    {
        state = State.Idle;
        model.GetComponent<Animator>().SetBool("walking", false);
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
    }

    void OnUndo()
    {
        if (state == State.Idle)
            ActionStack.Instance.Pop();
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        bool moved = false;

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

        if (moved)
        {
            lastMovementDirection = direction;

            if (moveController.Move(direction, 1))
            {
                state = State.Moving;
                model.GetComponent<Animator>().SetBool("walking", true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                HandleMovement();
                if (rb.velocity.y < fallingThreshold)
                {
                    state = State.Falling;
                }
                break;

            case State.Moving:
                // Handled by MoveController
                break;

            case State.Falling:
                if (rb.velocity.y > fallingThreshold)
                {
                    state = State.Idle;
                }
                break;
        }

        // Update model rotation
        Quaternion target = Quaternion.LookRotation(lastMovementDirection, Vector3.up);
        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, target, 800f * Time.deltaTime);
    }
}
