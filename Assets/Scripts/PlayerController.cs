using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform mainCamera;
    public float fallingThreshold = -0.5f;

    private enum State
    {
        Idle,
        Moving,
        Falling
    }

    private State state;
    private Vector2 movementVector;
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
    }

    void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
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

        if (moved && moveController.Move(direction, 1))
        {
            state = State.Moving;
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
    }
}
