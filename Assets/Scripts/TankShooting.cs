using UnityEngine;
using UnityEngine.Events;

public class TankShooting : MonoBehaviour
{
    [SerializeField] Transform cannonAxis;
    [SerializeField] float minProjectileSpeed = 10f;
    [SerializeField] float maxProjectileSpeed = 20f;
    [SerializeField] float minAimingAngle = 0f;
    [SerializeField] float maxAimingAngle = 180f;

    float currentProjectileSpeed;
    float currentAimingAngle;

    UnityEvent onSpeedChange = new UnityEvent();
    UnityEvent onAngleChange = new UnityEvent();

    void Awake()
    {
        currentProjectileSpeed = (minProjectileSpeed + maxProjectileSpeed) * 0.5f;
        currentAimingAngle = (minAimingAngle + maxAimingAngle) * 0.5f;
    }

    void Update()
    {
        if (Input.GetButton("Adjust Speed Modifier") && !Input.GetButton("Adjust Angle Modifier"))
        {
            float speedAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newSpeed = currentProjectileSpeed + speedAdjustment * Mathf.Abs(speedAdjustment * 100f);
            float previousSpeed = currentProjectileSpeed;
            
            currentProjectileSpeed = Mathf.Clamp(newSpeed, minProjectileSpeed, maxProjectileSpeed);
            if (currentProjectileSpeed != previousSpeed)
                onSpeedChange.Invoke();
        }
        
        if (Input.GetButton("Adjust Angle Modifier") && !Input.GetButton("Adjust Speed Modifier"))
        {
            float angleAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newAngle = currentAimingAngle + angleAdjustment * Mathf.Abs(angleAdjustment * 100f);
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