using System.Collections.Generic;
using UnityEngine;

public struct Tank
{
    public int index;
    public TankMovement movement;
    public TankShooting shooting;

    public Tank(int i, TankMovement m, TankShooting s)
    {
        index = i;
        movement = m;
        shooting = s;
    }
}

public enum ViewBound
{
    Top,
    Bottom,
    Left,
    Right
}

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    [SerializeField] GameObject tank1;
    [SerializeField] GameObject tank2;
    [SerializeField] float turnDuration = 60.0f;

    Camera gameCamera;
    Tank[] tanks = new Tank[2];
    Tank activeTank;
    Dictionary<ViewBound, float> viewBounds = new Dictionary<ViewBound, float>();
    float turnTime; 

    void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        else
        {
            gameCamera = FindObjectOfType<Camera>();

            viewBounds.Add(ViewBound.Top, gameCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y);
            viewBounds.Add(ViewBound.Bottom, gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y);
            viewBounds.Add(ViewBound.Left, gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x);
            viewBounds.Add(ViewBound.Right, gameCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);

            turnTime = turnDuration;
            
            tanks[0].index = 0;
            tanks[1].index = 1;
            tanks[0].movement = tank1.GetComponent<TankMovement>();
            tanks[1].movement = tank2.GetComponent<TankMovement>();
            tanks[0].shooting = tank1.GetComponent<TankShooting>();
            tanks[1].shooting = tank2.GetComponent<TankShooting>();

            tanks[0].movement.enabled = true;
            tanks[1].movement.enabled = false;
            tanks[0].shooting.enabled = true;
            tanks[1].shooting.enabled = false;

            activeTank = tanks[0];
            
            tanks[0].shooting.OnFireFinish.AddListener(ChangeTurns);
            tanks[1].shooting.OnFireFinish.AddListener(ChangeTurns);
        }
    }

    void Update()
    {
        if (!activeTank.shooting.IsFiring)
        {
            turnTime -= Time.deltaTime;
            if (turnTime <= 0f)
                ChangeTurns();
        }
    }

    void ChangeTurns()
    {
        turnTime = turnDuration;

        foreach (Tank tank in tanks)
        {
            tank.movement.enabled = !tank.movement.enabled;
            tank.shooting.enabled = !tank.shooting.enabled;

            if (tank.shooting.enabled)
            {
                tank.shooting.IsFiring = false;
                activeTank = tank;
            }
        }
    }

    public Tank ActiveTank
    {
        get { return activeTank; }
    }

    public float TurnTime
    {
        get { return turnTime; }
    }

    public Tank[] Tanks
    {
        get { return tanks; }
    }

    public float GetViewBound(ViewBound viewBound)
    {
        return viewBounds[viewBound];
    }

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<GameManager>();
                if (!instance)
                {
                    GameObject gameObj = new GameObject("Game Manager");
                    instance = gameObj.AddComponent<GameManager>();
                }
            }
            
            return instance;
        }
    }
}