using UnityEngine;
using UnityEngine.Events;

public class TankShooting : MonoBehaviour
{
    [SerializeField] Transform cannonAxis;
    [SerializeField] float minProjectileSpeed = 10f;
    [SerializeField] float maxProjectileSpeed = 20f;
    [SerializeField] float minAimingAngle = 0f;
    [SerializeField] float maxAimingAngle = 180f;
    [SerializeField] float speedAdjustmentIntervals = 2f;
    [SerializeField] float angleAdjustmentIntervals = 15f;

    float currentProjectileSpeed;
    float currentAimingAngle;

    UnityEvent onSpeedChange = new UnityEvent();
    UnityEvent onAngleChange = new UnityEvent();

    void Update()
    {
        if (!Input.GetButton("Adjust Angle Modifier"))
        {
            float speedAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newSpeed = currentProjectileSpeed + speedAdjustment * speedAdjustmentIntervals;
            float previousSpeed = currentProjectileSpeed;
            
            currentProjectileSpeed = Mathf.Clamp(newSpeed, minProjectileSpeed, maxProjectileSpeed);
            if (currentProjectileSpeed != previousSpeed)
                onSpeedChange.Invoke();
        }
        else
        {
            float angleAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newAngle = currentAimingAngle + angleAdjustment * angleAdjustmentIntervals;
            float previousAngle = currentAimingAngle;
            currentAimingAngle = Mathf.Clamp(newAngle, minAimingAngle, maxAimingAngle);

            cannonAxis.Rotate(0f, 0f, Mathf.Deg2Rad * (currentAimingAngle - previousAngle));

            if (currentAimingAngle != previousAngle)
                onAngleChange.Invoke();
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
    }

    public float CurrentAimingAngle
    {
        get { return currentAimingAngle; }
    }

    public UnityEvent OnSpeedChange
    {
        get { return onSpeedChange; }
    }

    public UnityEvent OnAngleChange
    {
        get { return onAngleChange; }
    }
}