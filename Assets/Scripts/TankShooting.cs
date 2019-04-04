using UnityEngine;

public class TankShooting : MonoBehaviour
{
    [SerializeField] float minProjectileSpeed = 10f;
    [SerializeField] float maxProjectileSpeed = 20f;
    [SerializeField] float minAimingAngle = 0f;
    [SerializeField] float maxAimingAngle = 180f;
    [SerializeField] float speedAdjustmentIntervals = 2f;
    [SerializeField] float angleAdjustmentIntervals = 15f;

    float currentProjectileSpeed;
    float currentAimingAngle;

    void Update()
    {
        if (!Input.GetButton("Adjust Angle Modifier"))
        {
            float speedAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newSpeed = currentProjectileSpeed + speedAdjustment * speedAdjustmentIntervals; 
            currentProjectileSpeed = Mathf.Clamp(newSpeed, minProjectileSpeed, maxProjectileSpeed);
        }
        else
        {
            float angleAdjustment = Input.GetAxis("Mouse ScrollWheel");
            float newAngle = currentAimingAngle + angleAdjustment * angleAdjustmentIntervals; 
            currentAimingAngle = Mathf.Clamp(newAngle, minAimingAngle, maxAimingAngle);
        }  
    }
}