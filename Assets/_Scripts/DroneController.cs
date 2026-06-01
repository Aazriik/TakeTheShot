using UnityEngine;

public class DroneController : MonoBehaviour
{
    PlayerControls input;
     void Awake()
    {
        input = new PlayerControls();
    }
     private void OnEnable()
    {
        input.Enable();
        input.DroneMode.Enable();
    }
     private void OnDisable()
    {
        input.Disable();
        input.DroneMode.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InputManager.Instance.OnDroneModeInput += HandleDroneModeInput;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
