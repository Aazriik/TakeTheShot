using UnityEngine;

[DefaultExecutionOrder(-100)] // Ensure this runs before most other scripts
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    //Public facing events and methods for other scripts to subscribe to or call
    public event System.Action OnPlayerModeInput;
    public event System.Action OnDroneModeInput;
    public event System.Action OnCameraModeInput;


    private PlayerControls input;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        input = new PlayerControls();
    }

    private void OnEnable()
    {
        input.Enable();
        //input.PlayerMode.Enable();
        input.DroneMode.Enable();
        //input.CameraMode.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        //input.PlayerMode.Disable();
        input.DroneMode.Disable();
        //input.CameraMode.Disable();
    }

}
