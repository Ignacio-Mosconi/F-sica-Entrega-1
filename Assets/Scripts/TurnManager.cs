using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static TurnManager instance;

    [SerializeField] GameObject tank1;
    [SerializeField] GameObject tank2;
    [SerializeField] float turnDuration = 60.0f;

    TankMovement tank1Movement;
    TankMovement tank2Movement;
    TankShooting tank1Shooting;
    TankShooting tank2Shooting;
    float turnTime; 

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        turnTime = turnDuration;
        
        tank1Movement = tank1.GetComponent<TankMovement>();
        tank2Movement = tank2.GetComponent<TankMovement>();
        tank1Shooting = tank1.GetComponent<TankShooting>();
        tank2Shooting = tank2.GetComponent<TankShooting>();

        tank1Movement.enabled = true;
        tank2Movement.enabled = false;
        tank1Shooting.enabled = true;
        tank2Shooting.enabled = false;
    }

    void Update()
    {
        turnTime -= Time.deltaTime;

        if (turnTime <= 0f)
            ChangeTurns();
    }

    void ChangeTurns()
    {
        turnTime = turnDuration;

        tank1Movement.enabled = !tank1Movement.enabled;
        tank2Movement.enabled = !tank2Movement.enabled;
        tank1Shooting.enabled = !tank1Shooting.enabled;
        tank2Shooting.enabled = !tank2Shooting.enabled;
    }

    public TankShooting GetActiveTankShooting()
    {
        TankShooting activeTankShooting;
        
        if (tank1Shooting.enabled)
            activeTankShooting = tank1Shooting;
        else
            activeTankShooting = tank2Shooting;

        return activeTankShooting;
    } 

    public static TurnManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<TurnManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Turn Manager");
                    instance = gameObj.AddComponent<TurnManager>();
                }
            }
            
            return instance;
        }
    }
}