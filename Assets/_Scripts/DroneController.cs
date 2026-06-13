using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;

    // Movement Parameters.
    //private float speed = 5f;
    private float powerUp = 0f;
    private float powerMax = 30f;
    private float gravity = -9.81f;
    private Vector3 tiltInput;
    private Vector3 powerVelocity;
    private Vector3 direction;

    // References.
    private Rigidbody rb;

    // Pause Menu Reference.
    public GameObject PauseDisplay;
    bool isPaused = false;

    #endregion

    #region Enable/Disable Action Maps
    private void OnEnable()
    {
        InputActions.FindActionMap("AM_Drone").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("AM_Drone").Disable();
    }

    #endregion


    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Get Component References.
        rb = GetComponent<Rigidbody>();
        bool isPaused = PauseDisplay.activeInHierarchy;

    }

    // Update is called once per frame
    void Update()
    {
        // Tilt.
        rb.rotation = Quaternion.Euler(rb.linearVelocity.y, rb.linearVelocity.x, rb.linearVelocity.z);

        #region Power and Gravity
        // PowerVelocity and Gravity.
        if (InputActions.FindAction("AM_Drone/PowerUp").IsPressed())
        {
            if (powerUp < powerMax)
            {
                powerUp += 0.3f;
            }
            if (powerUp > powerMax)
            {
                powerUp = powerMax;
            }

            rb.linearVelocity += Vector3.up * powerUp * Time.deltaTime;
            //Debug.Log("Linear Velocity is set at " + rb.linearVelocity);
        }
        else
        {
            powerUp += -0.05f;
            //Debug.Log("Linear Velocity is set at " + rb.linearVelocity);
        }

        if (powerUp < 0f)
        {
            powerUp = 0f;
        }

        rb.linearVelocity += Vector3.up * gravity * Time.deltaTime;

        #endregion


    }

    public void OnPowerUp(InputAction.CallbackContext context)
    {
        powerUp += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tiltInput = context.ReadValue<Vector2>();


        rb.AddTorque(tiltInput.x, tiltInput.y, tiltInput.z);
        //tiltInput.x = 
        
        // Apply tiltInput to the Drone's local Rotation.
        //transform.localRotation = Quaternion.Euler(tiltInput);
        //transform.forward
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        // Check if the game is Not Paused.
        if (!isPaused)
        {
            // Toggle the Pause Menu ON.
            PauseDisplay.SetActive(true);
            // Pause the game by setting time scale to 0.
            Time.timeScale = 0f;
            // If we are now paused, disable DRONE input and enable UI input.
            InputActions.FindActionMap("AM_Drone").Disable();
            InputActions.FindActionMap("AM_UI").Enable();
            // Set isPaused to the PauseDisplay active state.
            isPaused = PauseDisplay.activeInHierarchy;
            Debug.Log("Is Paused? " + isPaused);
        }
        else
        {
            // Toggle the Pause Menu OFF.
            PauseDisplay.SetActive(false);
            // Unpause the game by setting time scale back to 1.
            Time.timeScale = 1f;
            // If we are now unpaused, disable UI input and enable DRONE input.
            InputActions.FindActionMap("AM_UI").Disable();
            InputActions.FindActionMap("AM_Drone").Enable();
            // Set isPaused to the PauseDisplay active state.
            isPaused = PauseDisplay.activeInHierarchy;
            Debug.Log("Is Paused? " + isPaused);
        }
    }

    public void OnFreeze(InputAction.CallbackContext context)
    {
        bool isFrozen = rb.isKinematic;

        if (!isFrozen)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}
