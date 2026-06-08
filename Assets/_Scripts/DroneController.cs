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
    [SerializeField] float gravity = -9.8f;
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
            powerUp++; // Increase powerUp over time while the button is held down.
            //powerUp += Time.deltaTime; // Increase powerUp over time while the button is held down.
            powerVelocity.y += powerUp; // Apply the powerUp to the powerVelocity, allowing for a boost in upward movement.
        }
        else
        {
            powerUp = 0f; // Reset powerUp when the button is released.
        }
        powerVelocity.y += gravity * Time.deltaTime;
        controller.Move(powerVelocity * gravity *Time.deltaTime);

    }

    public void OnPowerUp(InputAction.CallbackContext context)
    {
        powerUp += context.ReadValue<float>() * Time.deltaTime;
        //Debug.Log($"PowerUp {context.performed}");
        if (context.performed)
        {
            // Read the powerUp value from the input context and apply it to the powerVelocity.
            //powerUp = context.ReadValue<float>();
            // Keep adding to powerUp while the input is held down
            //powerUp += context.ReadValue<float>() * Time.deltaTime;
            // Apply the powerUp to the powerVelocity, allowing for a boost in upward movement.
            //powerVelocity.y += powerUp * Time.deltaTime;
        }
        else
        {
            // Reset powerUp when the input is released.
            //powerUp = 0f;
        }
        
    }

    public void OnTilt(InputAction.CallbackContext context)
    {
        tiltInput = context.ReadValue<Vector3>();
        
        // Apply tiltInput to the Drone's local Rotation.
        transform.localRotation = Quaternion.Euler(tiltInput);
    }
}
