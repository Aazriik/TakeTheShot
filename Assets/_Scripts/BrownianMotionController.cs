using UnityEngine;
using Klak.Motion;
using DG.Tweening;

public class BrownianMotionController : MonoBehaviour
{
    // Variables
    private BrownianMotion motionController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        motionController = GetComponent<BrownianMotion>();
        motionController.positionFrequency = 0;
        motionController.rotationFrequency = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMotion()
    {
        DOTween.To(Starting, 0, 0.2f, 3);
    }

    void Starting(float value)
    {
        motionController.positionFrequency = value;
        motionController.rotationFrequency = value;
    }

    public void StopMotion()
    {
        DOTween.To(Stopping, motionController.positionFrequency, 0, 1);
    }

    void Stopping(float value)
    {
        motionController.positionFrequency = value;
        motionController.rotationFrequency = value;
    }
}
