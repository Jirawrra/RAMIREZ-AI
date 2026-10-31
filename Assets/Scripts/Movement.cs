using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public bool canInteract = false;
    public bool interacting = false;
    public GameObject WateringCanRenderer;

    public Direction direction;
    public Watering watering;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        direction = Direction.Right; // Default direction
        moveDirection = Vector2.right;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for interaction input
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!canInteract)
            {
                return;
            }
            interacting = true;
            watering.isWatering = true;
            //  WateringCanRenderer.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            if (!canInteract || watering == null)
            {
                return;
            }
            interacting = false;
            watering.isWatering = false;
            // WateringCanRenderer.SetActive(false);

        }

        Debug.Log(interacting);
        WateringCanRenderer.SetActive(interacting);

        if (interacting)
        {
            animator.SetBool("isWalking", false);

            return;
        }
        playerInput();
    }

    void FixedUpdate()
    {
        if (interacting)
        {
            return;
        }
        else
        {
            checkMovement();
        }
    }

    public void playerInput()
    {
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        int pressedCount = 0;
        if (up) pressedCount++;
        if (down) pressedCount++;
        if (left) pressedCount++;
        if (right) pressedCount++;

        if (pressedCount != 1)
        {
            animator.SetBool("isWalking", false);
            moveDirection = Vector2.zero;
            return;
        }

        animator.SetBool("isWalking", true);

        if (up)
        {
            direction = Direction.Up;
            moveDirection = Vector2.up;
        }
        else if (down)
        {
            direction = Direction.Down;
            moveDirection = Vector2.down;
        }
        else if (left)
        {
            direction = Direction.Left;
            moveDirection = Vector2.left;
        }
        else if (right)
        {
            direction = Direction.Right;
            moveDirection = Vector2.right;
        }

        animator.SetFloat("InputX", moveDirection.x);
        animator.SetFloat("InputY", moveDirection.y);

        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("LastInputX", moveDirection.x);
            animator.SetFloat("LastInputY", moveDirection.y);
        }
    }

    public void checkMovement()
    {
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }
}
