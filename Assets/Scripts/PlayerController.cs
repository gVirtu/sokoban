using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform mainCamera;
    public GameObject model;
    public float fallingThreshold = -0.5f;
    public float winAscendSpeed = 3f;
    public float winRotateSpeed = 80f;

    private enum State
    {
        Idle,
        Moving,
        Falling,
        Winning
    }

    private State state;
    private Vector2 movementVector;
    private Vector3 lastMovementDirection = Vector3.forward;
    private Rigidbody rb;
    private MoveController moveController;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        rb = GetComponent<Rigidbody>();
        animator = model.GetComponent<Animator>();
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
        if (state == State.Moving)
        {
            state = State.Idle;
        }
        animator.SetBool("walking", false);
    }

    public void HandleLevelVictory()
    {
        state = State.Winning;
        Instantiate(GameManager.Instance.Prefabs.lightBeam, moveController.moveTarget.transform.position, Quaternion.identity);
        rb.useGravity = false;
        animator.SetBool("walking", false);
        animator.SetBool("won", true);
        Invoke("HandleShowWinHUD", 1.5f);
    }

    private void HandleShowWinHUD()
    {
        GameObject winHud = GameObject.FindWithTag("WinHUD");
        winHud.transform.GetChild(0).gameObject.SetActive(true);
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
                animator.SetBool("walking", true);
            }
        }
    }
    void HandleFacingDirection()
    {
        Quaternion target = Quaternion.LookRotation(lastMovementDirection, Vector3.up);
        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, target, 800f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                HandleMovement();
                HandleFacingDirection();
                if (rb.velocity.y < fallingThreshold)
                {
                    state = State.Falling;
                }
                break;

            case State.Moving:
                // Handled by MoveController
                HandleFacingDirection();
                break;

            case State.Falling:
                if (rb.velocity.y > fallingThreshold)
                {
                    state = State.Idle;
                }
                break;

            case State.Winning:
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                {
                    transform.Translate(Vector3.up * winAscendSpeed * Time.deltaTime);
                    model.transform.Rotate(Vector3.up, winRotateSpeed * Time.deltaTime);
                }
                break;
        }

    }
}
