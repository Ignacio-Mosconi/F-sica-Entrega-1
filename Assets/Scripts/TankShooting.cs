using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CustomBoxCollider2D))]
public class TankShooting : MonoBehaviour
{
    enum CannonFacing
    {
        Left, Right
    }

    [SerializeField] Transform cannon;
    [SerializeField] GameObject cannonBall;
    [SerializeField] CannonFacing cannonFacing;
    [SerializeField] [Range(5f, 10f)] float minProjectileSpeed = 5f;
    [SerializeField] [Range(15f, 20f)] float maxProjectileSpeed = 20f;
    [SerializeField] [Range(0f, 90f)] float minAimingAngle = 0f;
    [SerializeField] [Range(135f, 180f)] float maxAimingAngle = 180f;
    [SerializeField] [Range(5f, 10f)] float shotDuration = 5f;

    CustomBoxCollider2D boxCollider;
    CustomCircleCollider2D cannonBallCollider;
    bool isFiring = false;
    float currentProjectileSpeed;
    float currentAimingAngle;

    UnityEvent onSpeedChange = new UnityEvent();
    UnityEvent onAngleChange = new UnityEvent();
    UnityEvent onFireFinish = new UnityEvent();
    UnityEvent onSpeedModifierPressed= new UnityEvent();
    UnityEvent onSpeedModifierReleased = new UnityEvent();
    UnityEvent onAngleModifierPressed= new UnityEvent();
    UnityEvent onAngleModifierReleased = new UnityEvent();

    void Awake()
    {
        boxCollider = GetComponent<CustomBoxCollider2D>();
        cannonBallCollider = cannonBall.GetComponent<CustomCircleCollider2D>();
        currentAimingAngle = cannon.rotation.z;
        currentProjectileSpeed = minProjectileSpeed;

        cannonBallCollider.OnTrigger.AddListener(OnCannonBallTriggerDetected);
        cannonBallCollider.CollisionEnabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Adjust Speed Modifier") && !Input.GetButton("Adjust Angle Modifier"))
            onSpeedModifierPressed.Invoke();
        if (Input.GetButtonUp("Adjust Speed Modifier"))
            onSpeedModifierReleased.Invoke();
        if (Input.GetButtonDown("Adjust Angle Modifier") && !Input.GetButton("Adjust Speed Modifier"))
            onAngleModifierPressed.Invoke();
        if (Input.GetButtonUp("Adjust Angle Modifier"))
            onAngleModifierReleased.Invoke();

        if (Input.GetButton("Adjust Speed Modifier") && !Input.GetButton("Adjust Angle Modifier"))
            AdjustProjectileSpeed();
        if (Input.GetButton("Adjust Angle Modifier") && !Input.GetButton("Adjust Speed Modifier"))
            AdjustAimingAngle();
        
        if (Input.GetButton("Fire") && !isFiring)
            LaunchProjectile();
    }

    void AdjustProjectileSpeed()
    {
        float speedAdjustment = Input.GetAxis("Mouse ScrollWheel");
        float newSpeed = currentProjectileSpeed + speedAdjustment * Mathf.Abs(speedAdjustment * 100f);
        float previousSpeed = currentProjectileSpeed;

        currentProjectileSpeed = Mathf.Clamp(newSpeed, minProjectileSpeed, maxProjectileSpeed);
        if (currentProjectileSpeed != previousSpeed)
            onSpeedChange.Invoke();
    }

    void AdjustAimingAngle()
    {   
        float angleAdjustment = Input.GetAxis("Mouse ScrollWheel");
        float newAngle = currentAimingAngle + angleAdjustment * Mathf.Abs(angleAdjustment * 100f);
        float previousAngle = currentAimingAngle;
        currentAimingAngle = Mathf.Clamp(newAngle, minAimingAngle, maxAimingAngle);

        if (cannonFacing == CannonFacing.Right)
            cannon.Rotate(0f, 0f, currentAimingAngle - previousAngle);
        else
            cannon.Rotate(0f, 0f, -(currentAimingAngle - previousAngle));

        if (currentAimingAngle != previousAngle)
            onAngleChange.Invoke();
    }

    void LaunchProjectile()
    {
        float projectileSpeed = (cannonFacing == CannonFacing.Right) ? currentProjectileSpeed : -currentProjectileSpeed;
        float aimingAngle = (cannonFacing == CannonFacing.Right) ? currentAimingAngle : -currentAimingAngle;

        ResetCannonBall();
        isFiring = true;

        StartCoroutine(ComputeProjectileTrajectory(projectileSpeed, aimingAngle, Physics2D.gravity.y));
    }

    void ResetCannonBall()
    {
        cannonBall.SetActive(false);
        cannonBallCollider.CollisionEnabled = false;
        cannonBall.transform.position = cannonBall.transform.parent.position;
    }

    IEnumerator ComputeProjectileTrajectory(float speed, float angle, float gravity)
    {
        float time = 0f;
        bool traveling = true;
        float initialSpeedX = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
        float initialSpeedY = speed * Mathf.Sin(angle * Mathf.Deg2Rad);
        float initialPosX = cannonBall.transform.position.x;
        float initialPosY = cannonBall.transform.position.y;

        cannonBall.SetActive(true);
        cannonBallCollider.CollisionEnabled = true;

        while (traveling)
        {
            time += Time.deltaTime;

            float posx = initialSpeedX * time + initialPosX;
            float posY = (gravity * time * time) * 0.5f + initialSpeedY * time + initialPosY;

            cannonBall.transform.position = new Vector3(posx, posY, cannonBall.transform.position.z);

            if (time > shotDuration || !cannonBall.activeInHierarchy)
                traveling = false;

            yield return null;
        }
        
        if (cannonBall.activeInHierarchy)
            ResetCannonBall();

        onFireFinish.Invoke();
    }

    void OnCannonBallTriggerDetected(CustomCollider2D collider)
    {
        if (collider != boxCollider)
        {
            ResetCannonBall();
        }
    }

    public bool IsFiring
    {
        get { return isFiring; }
        set { isFiring = value; }
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

    public UnityEvent OnFireFinish
    {
        get { return onFireFinish; }
    }

    public UnityEvent OnSpeedModifierPressed
    {
        get { return onSpeedModifierPressed; }
    }

    public UnityEvent OnSpeedModifierReleased
    {
        get { return onSpeedModifierReleased; }
    }

    public UnityEvent OnAngleModifierPressed
    {
        get { return onAngleModifierPressed; }
    }

    public UnityEvent OnAngleModifierReleased
    {
        get { return onAngleModifierReleased; }
    }
}