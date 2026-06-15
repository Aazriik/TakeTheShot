using UnityEngine;

public class BladesController : MonoBehaviour
{
    // Variables
    // Enum to hold Main 3 Axis which will be selected from for each blade individually.
    public enum Axis
    {
        x,
        y,
        z
    }
    public Axis rotationAxis;
    private float bladeSpeed;
    public float BladeSpeed
    {
        get {  return bladeSpeed; }
        set { bladeSpeed = Mathf.Clamp(value, 0, 3000); }
    }
    public bool inverseRotation = false;
    private Vector3 Rotation;
    private float rotateDegree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (inverseRotation) rotateDegree -= bladeSpeed * Time.deltaTime;
        else rotateDegree += bladeSpeed * Time.deltaTime;

        rotateDegree = rotateDegree % 360;

        // Switch Condition will apply rotation for each axis on its LOCAL rotation using Quaternion.Euler * Rotation Speed.
        switch(rotationAxis)
        {
            case Axis.y: // Use rotateDegree in the Y-Axis
                transform.localRotation = Quaternion.Euler(Rotation.x, rotateDegree, Rotation.z);
                break;
            case Axis.z: // Use rotateDegree in the Z-Axis
                transform.localRotation = Quaternion.Euler(Rotation.x, Rotation.y, rotateDegree);
                break;
            default: // Use rotateDegree in the X-Axis
                transform.localRotation = Quaternion.Euler(rotateDegree, Rotation.y, Rotation.z);
                break;
        }
    }
}
