using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;

    // Player Input Actions.
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction droneModeAction;

    // Pause Actions for All Action Maps.
    private InputAction pauseActionPlayer;
    private InputAction pauseActionUI;

    // References.
    private CharacterController controller;

    // Movement Parameters.
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.8f;
    private Vector3 moveInput;
    private Vector3 velocity;

    // Pause Menu Reference.
    public GameObject PauseDisplay;
    

    #endregion

    #region Enable/Disable Action Maps
    private void OnEnable()
    {
        InputActions.FindActionMap("AM_Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("AM_Player").Disable();
    }

    #endregion


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Initialize Player Input Actions.
        moveAction = InputSystem.actions.FindAction("AM_Player/Move");
        // Initialize Look Action for Player Action Map.
        lookAction = InputSystem.actions.FindAction("AM_Player/Look");
        // Initialize Jump Action for Player Action Map.
        jumpAction = InputSystem.actions.FindAction("AM_Player/Jump");
        // Initialize Drone Mode Action for Player Action Map.
        droneModeAction = InputSystem.actions.FindAction("AM_Player/DroneMode");

        // Initialize Pause Actions for Player and UI Action Maps.
        pauseActionPlayer = InputSystem.actions.FindAction("AM_Player/Pause");
        pauseActionUI = InputSystem.actions.FindAction("AM_UI/Pause");

        // Get Component References.
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame.
    void Update()
    {
        // Movement.
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(moveDirection * walkSpeed * Time.deltaTime);

        // Jumping and Gravity.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // OnMove Input Action Callback using Player Input Component with Invoking Unity Events.
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");

        // Get camera normalized directional vectors.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        // Create direction-relative-input vectors.
        Vector3 forwardRelativeVerticalInput = forward * moveInput.y;
        Vector3 rightRelativeHorizontalInput = right * moveInput.x;

        // Create camera-relative movement.
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        controller.Move(cameraRelativeMovement * walkSpeed * Time.deltaTime);

        // Create and apply camera relative movement.
        //Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        //this.transform.Translate(cameraRelativeMovement, Space.World);
    }

    // OnLook Input Action Callback using Player Input Component with Invoking Unity Events.
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        // Debug.Log($"Look Input: {lookInput}");
        // Handle player rotation and forward move direction based on look input here.

        // Get camera normalized directional vectors.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        // 0 the Y-Axis to prevent the player moving up/down when the camera is tilted.
        forward.y = 0;
        right.y = 0;
        // Normalize the vectors to ensure consistent move speed in all directions.
        forward = forward.normalized;
        right = right.normalized;
        // Create direction-relative-input vectors.
        Vector3 forwardRelativeVerticalInput = forward * moveInput.y;
        Vector3 rightRelativeHorizontalInput = right * moveInput.x;
        // Create camera-relative movement.
        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        // Rotate the player to face the direction of movement.
        if (cameraRelativeMovement.sqrMagnitude > 0.01f)
        {
            // Create a target rotation based on the camera-relative movement direction.
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMovement);
            // Smoothly rotate the player towards the target rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    // OnJump Input Action Callback using Player Input Component with Invoking Unity Events.
    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed && controller.isGrounded)
        {
            //Debug.Log("We are supposed to jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // OnPause Input Action Callback using Player Input Component with Invoking Unity Events.
    public void OnPause(InputAction.CallbackContext context)
    {
        // Check if the Pause action was triggered in the Player Action Map.
        if (context.performed)
        {
            // Toggle the Pause Menu and switch input between Action Maps.
            bool isPaused = PauseDisplay.activeSelf;
            PauseDisplay.SetActive(!isPaused);
            if (!isPaused)
            {
                // If we are now paused, disable Player input and enable UI input.
                InputActions.FindActionMap("AM_Player").Disable();
                InputActions.FindActionMap("AM_UI").Enable();

                // Pause the game by setting time scale to 0.
                Time.timeScale = 0f;
            }
            else
            {
                // If we are now unpaused, disable UI input and enable Player input.
                InputActions.FindActionMap("AM_UI").Disable();
                InputActions.FindActionMap("AM_Player").Enable();

                // Unpause the game by setting time scale back to 1.
                Time.timeScale = 1f;
            }
        }
    }
}
