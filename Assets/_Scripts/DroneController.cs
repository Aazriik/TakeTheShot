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
    public float engineLift = 0.0075f;
    // Forward Movement Speed.
    public float forwardForce;
    // Backward Movement Speed.
    public float backwardForce;

    // Turning.
    public float turnForce;
    private float turnForceHelper;
    private float turning = 0f;

    // Vector 2 to Handle Movement.
    private Vector2 movement = Vector2.zero;

    // Ground Check.
    public LayerMask groundLayer;
    private float distanceToGround;
    public bool isGrounded = true;

    // Component References.
    private Rigidbody rb;

    // Pause Menu References.
    public GameObject pauseDisplay;
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
        bool isPaused = pauseDisplay.activeInHierarchy;

    }

    // Update is called once per frame.
    void Update()
    {
        HandleGroundCheck();
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
        if (!isGrounded)
        {
            //Debug.Log("Is NOT Grounded");

            // Movement (Horizontal and Vertical).
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            #region Movement Input Actions
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

            #endregion
        }

        // Ascend.
        if (InputActions.FindAction("AM_Drone/Ascend").IsPressed())
        {
            EnginePower += engineLift;
        }

        // Descend.
        if (InputActions.FindAction("AM_Drone/Descend").IsPressed())
        {
            EnginePower -= engineLift;

            if (EnginePower < 0) EnginePower = 0;
        }
        //Debug.Log("Is Grounded");

    }

    void HandleGroundCheck()
    {
        // Create Raycast shooting downward.
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        Ray ray = new Ray(transform.position, direction);

        // Check if the Raycast hits the Ground Layer Mask within 3000 units.
        if (Physics.Raycast(ray, out hit, 3000, groundLayer))
        {
            distanceToGround = hit.distance;

            // Check if Ground is less than 2 units away and set IsGrounded to True.
            if (distanceToGround < 0.2f) isGrounded = true;
            else isGrounded = false;
        }

    }

    // Ascend with Upward Force using Rigidbody while factoring in mass.
    void DroneHover()
    {
        if (InputActions.FindAction("AM_Drone/Ascend").IsPressed())
        {
            //EnginePower += engineLift;

            float upForce = 1 - Mathf.Clamp(rb.transform.position.y / effectiveHeight, 0, 1);
            upForce = Mathf.Lerp(0, EnginePower, upForce) * rb.mass;
            rb.AddRelativeForce(Vector3.up * upForce);
        }
        //float upForce = 1 - Mathf.Clamp(rb.transform.position.y / effectiveHeight, 0, 1);
        //upForce = Mathf.Lerp(0, EnginePower, upForce) * rb.mass;
        //rb.AddRelativeForce(Vector3.up * upForce);
        
    }

    void DroneMovements()
    {
        // Move Forward Z-Axis.
        if (InputActions.FindAction("AM_Drone/MoveForward").IsPressed())
        {
            rb.AddRelativeForce(Vector3.forward * Mathf.Max(0f, movement.y * forwardForce * rb.mass));
        }
        // Move Backward Z-Axis.
        else if (InputActions.FindAction("AM_Drone/MoveBackward").IsPressed())
        {
            rb.AddRelativeForce(Vector3.back * Mathf.Max(0f, -movement.y * backwardForce * rb.mass));
        }
        // Rotate Right.
        if (InputActions.FindAction("AM_Drone/MoveRight").IsPressed())
        {
            // Drone Turns RIGHT.
            float turn = turnForce * Mathf.Lerp(movement.x, movement.x * (turnForceHelper - Mathf.Abs(movement.y)), Mathf.Max(0f, movement.y));
            turning = Mathf.Lerp(turning, turn, Time.fixedDeltaTime * turnForce);
            rb.AddRelativeTorque(1f, movement.x * rb.mass, 0f);
        }
        // Rotate Left.
        if (InputActions.FindAction("AM_Drone/MoveLeft").IsPressed())
        {
            // Drone Turns LEFT.
            float turn = turnForce * Mathf.Lerp(movement.x, movement.x * (turnForceHelper - Mathf.Abs(movement.y)), Mathf.Max(0f, movement.y));
            turning = Mathf.Lerp(turning, turn, Time.fixedDeltaTime * turnForce);
            rb.AddRelativeTorque(0f, movement.x * rb.mass, 0f);
        }
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        movement.y += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        movement.y += context.ReadValue<float>() * Time.deltaTime;
    }

    public void OnMoveForward(InputAction.CallbackContext context)
    {
        
    }

    public void OnMoveBackward(InputAction.CallbackContext context)
    {
        
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        movement.x += context.ReadValue<float>() * Time.fixedDeltaTime;
    }

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        movement.x += context.ReadValue<float>() * Time.fixedDeltaTime;
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
            pauseDisplay.SetActive(true);
            // Pause the game by setting time scale to 0.
            Time.timeScale = 0f;
            // If we are now paused, disable DRONE input and enable UI input.
            InputActions.FindActionMap("AM_Drone").Disable();
            InputActions.FindActionMap("AM_UI").Enable();
            // Set isPaused to the PauseDisplay active state.
            isPaused = pauseDisplay.activeInHierarchy;
            Debug.Log("Is Paused? " + isPaused);
        }
        else
        {
            // Toggle the Pause Menu OFF.
            pauseDisplay.SetActive(false);
            // Unpause the game by setting time scale back to 1.
            Time.timeScale = 1f;
            // If we are now unpaused, disable UI input and enable DRONE input.
            InputActions.FindActionMap("AM_UI").Disable();
            InputActions.FindActionMap("AM_Drone").Enable();
            // Set isPaused to the PauseDisplay active state.
            isPaused = pauseDisplay.activeInHierarchy;
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
