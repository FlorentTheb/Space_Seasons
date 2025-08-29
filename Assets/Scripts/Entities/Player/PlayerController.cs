using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private Animator ControlAnimator;
    [SerializeField] private float laneDelta = 7f;
    [SerializeField] private float timeToMove = 0.1f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float currentTimer = 0f;
    private bool isMoving = false;
    void Awake()
    {
        ControlAnimator = GetComponent<Animator>();
        playerInputs = new PlayerInputs();
    }

    void OnEnable()
    {
        playerInputs.Player.Enable();
        playerInputs.Player.Jump.performed += ctx => OnJump();
        playerInputs.Player.Crouch.performed += ctx => OnCrouch();
        playerInputs.Player.Left.performed += ctx => TurnLeft();
        playerInputs.Player.Right.performed += ctx => TurnRight();
    }

    void OnDisable()
    {
        playerInputs.Player.Jump.performed -= ctx => OnJump();
        playerInputs.Player.Crouch.performed -= ctx => OnCrouch();
        playerInputs.Disable();
    }

    void Update()
    {
        if (isMoving)
            Move();
    }

    private void OnJump()
    {
        ControlAnimator.SetBool("IsJumping", true);
    }

    private void OnCrouch()
    {
        ControlAnimator.SetBool("IsCrouching", true);
    }

    private void TurnLeft()
    {
        TryMove(-1);
    }

    private void TurnRight()
    {
        TryMove(1);
    }


    private void TryMove(int direction)
    {
        if (isMoving) return;

        if ((direction == -1 && transform.position.x <= -14) ||
            (direction == 1 && transform.position.x >= 14))
            return;

        isMoving = true;
        startPos = transform.position;
        targetPos = new Vector3(transform.position.x + (laneDelta * direction), transform.position.y, transform.position.z);
    }

    private void Move()
    {
        currentTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTimer / timeToMove);

        transform.position = Vector3.Lerp(startPos, targetPos, progress);

        if (progress >= 1f)
        {
            transform.position = targetPos;
            currentTimer = 0f;
            isMoving = false;
        }
    }
}
