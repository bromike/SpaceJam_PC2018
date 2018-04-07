using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    [SerializeField]
    private int walkSpeed;
    [SerializeField]
    private int runSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float groundDistance;
    [SerializeField]
    private float mouseSensibility;
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private float Stamina;


    //Camera Settings
    private float Yaxis;
    private float Xaxis;
    private Quaternion cameraRotation;

    //Animation bools
    private bool isRunning;
    private bool isWalking;
    private bool isJumping;

    //Player Comps.
    private Camera playerCamera;
    private Rigidbody playerBody;
    private Vector3 playerMove;
    private PlayerBehaviour playerBehaviour;


    void Start()
    {
        Cursor.visible = false;                                             //This must be pass to the state machine of the game. Temporary placing for testing purposes.
        Cursor.lockState = CursorLockMode.Locked;

        playerBody = GetComponent<Rigidbody>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
        playerCamera = Camera.main;
        Yaxis = 0.0f;
        Xaxis = 0.0f;
        cameraRotation = new Quaternion(Xaxis, Yaxis, 0, 0);                //Initial rotation of the player.
        isRunning = false;
        isWalking = true;
        isJumping = false;
    }

    void FixedUpdate()
    {
        playerMove = playerBody.transform.forward * Input.GetAxis("Vertical") + playerBody.transform.right * Input.GetAxis("Horizontal");
        playerMove *= walkSpeed;
        playerBody.MovePosition(playerBody.transform.position + playerMove * Time.deltaTime);

        // Jumps
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            playerBody.velocity = Vector3.up * jumpHeight;
        }
        updateStamina(playerMove);

        if (Input.GetButton("Run") && Stamina > 0.0f)
        {
            playerMove = playerBody.transform.forward * Input.GetAxis("Vertical") + playerBody.transform.right * Input.GetAxis("Horizontal");
            playerBody.MovePosition(playerBody.transform.position + playerMove * Time.deltaTime);

            // Jumps
            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                playerBody.velocity = Vector3.up * jumpHeight;
            }
            updateStamina(playerMove);
        }

        Xaxis = Input.GetAxis("Mouse X") * mouseSensibility;
        Yaxis = Input.GetAxis("Mouse Y") * mouseSensibility;

        /* CAMERA ++ */
        

        /* CAMERA -- */
        
        if (Input.GetButton("Attack"))
        {
            //Attack behaviour here.
        }
    }

    public bool isGrounded()
    {
        bool Grounded;

        if (Physics.Raycast(playerBody.transform.position, Vector3.down, playerHeight))
            Grounded = true;
        else
            Grounded = false;

        return Grounded;
    }

    public Vector3 GetVelocity()
    {
        return playerBody.velocity;
    }

    void updateStamina(Vector3 Move)
    {
        if (Move != Vector3.zero && Input.GetButton("Run"))
            Stamina -= 1 * Time.deltaTime;
        else
            Stamina += 1 * Time.deltaTime;
        Stamina = Mathf.Clamp(Stamina, 0, 10);
    }

    public Camera GetPlayerCamera()
    {
        return playerCamera;
    }

}


