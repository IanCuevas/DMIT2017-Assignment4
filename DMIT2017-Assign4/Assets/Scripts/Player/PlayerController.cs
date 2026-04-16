using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement Values")]
    //Moving
    float movementSpeed = 7.0f;
    float movement;
    //Jumping
    float jumpForce = 7.0f;
    public int canDoubleJump = 0;

    [Header("GroundCheck")]
    public Transform groundCheckTransform;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Miscellaneous")]
    //Accessing InputSystem
    public InputSystem_Actions playerInput;
    //Accessing gameobject's reigidbody2D
    Rigidbody2D rb;

    [Header("Flip Character")]
    bool isFacingRight = true;

    void Awake()
    {
        playerInput = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        //When player is pressing on WASD/Joystick
        playerInput.Player.Move.performed += PlayerMovement;
        playerInput.Player.Jump.performed += PlayerJumping;
        //When player releases pressure on WASD/Joystick
        playerInput.Player.Move.canceled += PlayerMovement;
        playerInput.Player.Jump.canceled += PlayerJumping;

    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
        playerInput.Player.Move.performed -= PlayerMovement;
        playerInput.Player.Jump.performed -= PlayerJumping;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement
        //Taking in the X value of playerinput and multiplying it by movementspeed.
        rb.linearVelocityX = movement * movementSpeed;

        //Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);

    }

    void PlayerMovement(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>().x;
        if (movement > 0 && !isFacingRight)
        {
            FlipCharacter();
            Debug.Log("Right");
        }
        else if (movement < 0 && isFacingRight)
        {
            FlipCharacter();
            Debug.Log("Left");
        }
    }
    void PlayerJumping(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //this if statement checks if the player is grounded or can still perform another jump in the air
            if (isGrounded || canDoubleJump < 1)
            {
                rb.linearVelocityY = jumpForce;
                canDoubleJump++;
            }
            //When the player returns to the ground, double jump resets
            if (isGrounded)
            {
                canDoubleJump = 0;
            }
        }
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}

