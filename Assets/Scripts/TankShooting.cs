using UnityEngine;
using UnityEngine.Events;

public class TankShooting : MonoBehaviour
{
    [SerializeField] Transform cannon;
    [SerializeField] [Range(10f, 15f)] float minProjectileSpeed = 10f;
    [SerializeField] [Range(20f, 25f)] float maxProjectileSpeed = 25f;
    [SerializeField] [Range(-180f, 0f)] float minAimingAngle = 0f;
    [SerializeField] [Range(0f, 180f)] float maxAimingAngle = 180f;

    float currentProjectileSpeed;
    float currentAimingAngle;

    UnityEvent onSpeedChange = new UnityEvent();
    UnityEvent onAngleChange = new UnityEvent();

    void Awake()
    {
        currentAimingAngle = cannon.rotation.z;
        currentProjectileSpeed = minProjectileSpeed;
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

            cannon.Rotate(0f, 0f, currentAimingAngle - previousAngle);

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
        get { return Mathf.Abs(currentAimingAngle); }
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