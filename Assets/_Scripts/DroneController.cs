using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;

    // Movement Parameters.
    [SerializeField] float speed = 5f;
    [SerializeField] float powerUp = 0f;
    [SerializeField] float tempPower;
    [SerializeField] float gravity = -9.81f;
    private Vector3 tiltInput;
    private Vector3 powerVelocity;

    // References.
    private CharacterController controller;

    // Pause Menu Reference.
    public GameObject PauseDisplay;

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
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Tilt.

        // PowerVelocity and Gravity.
        if (InputActions.FindAction("AM_Drone/PowerUp").IsPressed())
        {
            powerUp += 0.001f; // Increase powerUp over time while the button is held down.
            //powerUp += Time.deltaTime; // Increase powerUp over time while the button is held down.
            powerVelocity.y += powerUp; // Apply the powerUp to the powerVelocity, allowing for a boost in upward movement.
        }
        else
        {
            powerUp = 0f; // Reset powerUp when the button is released.
        }
        powerVelocity.y += gravity * Time.deltaTime;
        controller.Move(powerVelocity * Time.deltaTime);

    }

    public void OnPowerUp(InputAction.CallbackContext context)
    {
        powerUp += context.ReadValue<float>() * Time.deltaTime;
        //Debug.Log($"PowerUp {context.performed}");
        //if (context.performed)
        //{
        //    powerUp += 0.01f; // Increase powerUp over time while the button is held down.
        //    //powerUp += Time.deltaTime; // Increase powerUp over time while the button is held down.
        //    powerVelocity.y += powerUp; // Apply the powerUp to the powerVelocity, allowing for a boost in upward movement.
        //}
        //else
        //{
        //    powerUp = 0f; // Reset powerUp when the button is released.
        //}
        
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tiltInput = context.ReadValue<Vector3>();
        
        // Apply tiltInput to the Drone's local Rotation.
        transform.localRotation = Quaternion.Euler(tiltInput);
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
                InputActions.FindActionMap("AM_Drone").Disable();
                InputActions.FindActionMap("AM_UI").Enable();

                // Pause the game by setting time scale to 0.
                Time.timeScale = 0f;
            }
            else
            {
                // If we are now unpaused, disable UI input and enable Player input.
                InputActions.FindActionMap("AM_UI").Disable();
                InputActions.FindActionMap("AM_Drone").Enable();

                // Unpause the game by setting time scale back to 1.
                Time.timeScale = 1f;
            }
        }
    }
}
