using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;

    // Player Input Actions.
    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;
    private InputAction m_droneModeAction;

    // Pause Actions for All Action Maps.
    private InputAction m_pauseActionPlayer;
    //private InputAction m_pauseActionDrone;
    //private InputAction m_pauseActionCamera;
    private InputAction m_pauseActionUI;

    // Movement and Look Amounts.
    private Vector2 m_moveAmt;
    private Vector2 m_lookAmt;

    // Component References.
    private Rigidbody m_rigidbody;

    // Movement Parameters.
    public float walkSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpSpeed = 5f;

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
        m_moveAction = InputSystem.actions.FindAction("AM_Player/Move");
        m_lookAction = InputSystem.actions.FindAction("AM_Player/Look");
        m_jumpAction = InputSystem.actions.FindAction("AM_Player/Jump");
        m_droneModeAction = InputSystem.actions.FindAction("AM_Player/DroneMode");

        // Initialize Pause Actions for Player and UI Action Maps.
        m_pauseActionPlayer = InputSystem.actions.FindAction("AM_Player/Pause");
        m_pauseActionUI = InputSystem.actions.FindAction("AM_UI/Pause");

        // Get Rigidbody Component.
        m_rigidbody = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        // Read Movement and Look Inputs.
        m_moveAmt = m_moveAction.ReadValue<Vector2>();
        m_lookAmt = m_lookAction.ReadValue<Vector2>();

        // Check for Jump Input.
        if (m_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }

        // Toggle Pause Menu.
        DisplayPause();
    }

    private void FixedUpdate()
    {
        // Run Walking and Rotating in FixedUpdate for consistent Physics behaviour.
        Walking();
        Rotating();
    }

    private void DisplayPause()
    {
        // Check if the Pause action was triggered in the Player Action Map.
        if (m_pauseActionPlayer.WasPressedThisFrame())
        {
            // Activate the Pause Menu and switch input to the UI Action Map.
            PauseDisplay.SetActive(true);
            InputActions.FindActionMap("AM_Player").Disable();
            InputActions.FindActionMap("AM_UI").Enable();
        }
        // Check if the Pause action was triggered in the UI Action Map to unpause.
        else if (m_pauseActionUI.WasPressedThisFrame())
        {
            // Deactivate the Pause Menu and switch input back to the Player Action Map.
            PauseDisplay.SetActive(false);
            InputActions.FindActionMap("AM_UI").Disable();
            InputActions.FindActionMap("AM_Player").Enable();
        }
    }

    public void Jump()
    {
        // Apply an upward force at the player's position to simulate a jump.
        m_rigidbody.AddForceAtPosition(new Vector3(0, 5f, 0), Vector3.up, ForceMode.Impulse);
    }

    private void Walking()
    {
        // Move the player forward based on the vertical input and walk speed.
        m_rigidbody.MovePosition(m_rigidbody.position + transform.forward * m_moveAmt.y * walkSpeed * Time.deltaTime);
    }

    private void Rotating()
    {
        // Rotate the player around the Y-axis based on the horizontal input and rotate speed.
        float rotationAmount = m_lookAmt.x * rotateSpeed * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);
        m_rigidbody.MoveRotation(m_rigidbody.rotation * deltaRotation);
    }
}
