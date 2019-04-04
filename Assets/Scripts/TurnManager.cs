using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static TurnManager instance;

    [SerializeField] GameObject tank1;
    [SerializeField] GameObject tank2;
    [SerializeField] float turnDuration = 60.0f;

    TankMovement tank1Movement;
    TankMovement tank2Movement;
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

        tank1Movement.enabled = true;
        tank2Movement.enabled = false;
    }

    void Update()
    {
        turnTime -= Time.deltaTime;

        Debug.Log(turnTime);

        if (turnTime <= 0f)
            ChangeTurns();
    }

    void ChangeTurns()
    {
        turnTime = turnDuration;

        tank1Movement.enabled = !tank1Movement.enabled;
        tank2Movement.enabled = !tank2Movement.enabled;
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