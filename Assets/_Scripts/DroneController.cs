using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{

    #region Variables
    // Reference to the Input Action Asset.
    public InputActionAsset InputActions;
    
    // Blade Controllers.
    public BladesController FR_Blade;
    public BladesController FL_Blade;
    public BladesController BR_Blade;
    public BladesController BL_Blade;

    // Engine Power.
    private float enginePower;
    public float EnginePower
    {
        get { return enginePower; }
        set
        {
            FR_Blade.BladeSpeed = value * 500;
            FL_Blade.BladeSpeed = value * 500;
            BR_Blade.BladeSpeed = value * 500;
            BL_Blade.BladeSpeed = value * 500;
            enginePower = value;
        }
    }

    // Effective Height determines how far the Drone will fly in the UP direction.
    public float effectiveHeight;
    // Engine Lift is Throttle Power.
    public float EngineLift = 0.0075f;
    // Forward Movement Speed.
    public float ForwardForce;
    // Backward Movement Speed.
    public float BackwardForce;

    // Vector 2 to Handle Movement.
    private Vector2 movement = Vector2.zero;

    // Component References.
    private Rigidbody rb;

    // Pause Menu References.
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


    // Start is called before the first frame update.
    void Start()
    {
        // Get Component References.
        rb = GetComponent<Rigidbody>();
        bool isPaused = PauseDisplay.activeInHierarchy;

    }

    // Update is called once per frame.
    void Update()
    {
        HandleInputs();
    }

    // This function is called every fixed frame, if the MonoBehaviour is enabled.
    protected void FixedUpdate()
    {
        DroneHover();
        DroneMovements();
    }

    // Function to handle all inputs in the Update Method.
    void HandleInputs()
    {
        // Movement (Horizontal and Vertical).
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        // Move Forward.
        if (InputActions.FindAction("AM_Drone/MoveForward").IsPressed())
        {
            
        }

        // Move Backward.
        if (InputActions.FindAction("AM_Drone/MoveBackward").IsPressed())
        {

        }

        // Move Left.
        if (InputActions.FindAction("AM_Drone/MoveLeft").IsPressed())
        {
            
        }

        // Move Right.
        if (InputActions.FindAction("AM_Drone/MoveRight").IsPressed())
        {

        }

        // Descend.
        if (InputActions.FindAction("AM_Drone/Descend").IsPressed())
        {
            EnginePower -= EngineLift;

            if (EnginePower < 0) EnginePower = 0;
        }

        // Ascend.
        if (InputActions.FindAction("AM_Drone/Ascend").IsPressed())
        {
            EnginePower += EngineLift;
        }
    }

    // Ascend with Upward Force using Rigidbody while factoring in mass.
    void DroneHover()
    {
        float upForce = 1 - Mathf.Clamp(rb.transform.position.y / effectiveHeight, 0, 1);
        upForce = Mathf.Lerp(0, EnginePower, upForce) * rb.mass;
        rb.AddRelativeForce(Vector3.up * upForce);
        
    }

    void DroneMovements()
    {
        // Move Forward Z-Axis.
        if (InputActions.FindAction("AM_Drone/MoveForward").IsPressed())
        {
            rb.AddRelativeForce(Vector3.forward * Mathf.Max(0f, movement.y * ForwardForce * rb.mass));
        }
        // Move Backward Z-Axis.
        else if (InputActions.FindAction("AM_Drone/MoveBackward").IsPressed())
        {
            rb.AddRelativeForce(Vector3.back * Mathf.Max(0f, movement.y * BackwardForce * rb.mass));
        }
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        movement.y += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        movement.y -= context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnMoveForward(InputAction.CallbackContext context)
    {
        
    }

    public void OnMoveBackward(InputAction.CallbackContext context)
    {

    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {

    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {

    }


    #region First Draft

    // Update is called once per frame
    //void Update()
    //{
    //    // Tilt.
    //    rb.rotation = Quaternion.Euler(rb.linearVelocity.y, rb.linearVelocity.x, rb.linearVelocity.z);

    //    #region Power and Gravity
    //    // PowerVelocity and Gravity.
    //    if (InputActions.FindAction("AM_Drone/PowerUp").IsPressed())
    //    {
    //        if (powerUp < powerMax)
    //        {
    //            powerUp += 0.3f;
    //        }
    //        if (powerUp > powerMax)
    //        {
    //            powerUp = powerMax;
    //        }

    //        rb.linearVelocity += Vector3.up * powerUp * Time.deltaTime;
    //        //Debug.Log("Linear Velocity is set at " + rb.linearVelocity);
    //    }
    //    else
    //    {
    //        powerUp += -0.05f;
    //        //Debug.Log("Linear Velocity is set at " + rb.linearVelocity);
    //    }

    //    if (powerUp < 0f)
    //    {
    //        powerUp = 0f;
    //    }

    //    rb.linearVelocity += Vector3.up * gravity * Time.deltaTime;

    //    #endregion


    //}

    //public void OnPowerUp(InputAction.CallbackContext context)
    //{
    //    powerUp += context.ReadValue<float>() * Time.deltaTime;
    //}

    //public void OnTilt(InputAction.CallbackContext context)
    //{
    //    tiltInput = context.ReadValue<Vector2>();


    //    rb.AddTorque(tiltInput.x, tiltInput.y, tiltInput.z);
    //    //tiltInput.x = 

    //    // Apply tiltInput to the Drone's local Rotation.
    //    //transform.localRotation = Quaternion.Euler(tiltInput);
    //    //transform.forward
    //}

    #endregion

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
