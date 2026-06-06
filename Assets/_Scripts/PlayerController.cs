using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;

    // Player Input Actions.
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction droneModeAction;

    // Pause Actions for All Action Maps.
    private InputAction pauseActionPlayer;
    private InputAction pauseActionUI;

    // Component References.
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


    private void Awake()
    {
        // Initialize Player Input Actions.
        moveAction = InputSystem.actions.FindAction("AM_Player/Move");
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

    // Update is called once per frame
    void Update()
    {
        // Toggle Pause Menu.
        //DisplayPause();

        // Movement.
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Jumping and Gravity.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jumping {context.performed} - Is Grounded: {controller.isGrounded}");
        if (context.performed && controller.isGrounded)
        {
            //Debug.Log("We are supposed to jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

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


    private void DisplayPause()
    {
        // Check if the Pause action was triggered in the Player Action Map.
        if (pauseActionPlayer.WasPressedThisFrame())
        {
            // Activate the Pause Menu and switch input to the UI Action Map.
            PauseDisplay.SetActive(true);
            InputActions.FindActionMap("AM_Player").Disable();
            InputActions.FindActionMap("AM_UI").Enable();
        }
        // Check if the Pause action was triggered in the UI Action Map to unpause.
        else if (pauseActionUI.WasPressedThisFrame())
        {
            // Deactivate the Pause Menu and switch input back to the Player Action Map.
            PauseDisplay.SetActive(false);
            InputActions.FindActionMap("AM_UI").Disable();
            InputActions.FindActionMap("AM_Player").Enable();
        }
    }
}
