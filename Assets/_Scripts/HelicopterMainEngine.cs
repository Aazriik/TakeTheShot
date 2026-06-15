using UnityEngine;

public class HelicopterMainEngine : MonoBehaviour
{
    // Variables
    public BladesController FR_Blade;
    public BladesController FL_Blade;
    public BladesController BR_Blade;
    public BladesController BL_Blade;

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
    public float EngineLift = 0.0075f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
