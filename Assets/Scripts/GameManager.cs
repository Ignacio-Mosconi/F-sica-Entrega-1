using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct Tank
{
    public int index;
    public TankMovement movement;
    public TankShooting shooting;
    public TankAnimation animation;
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
    int[] playerScores = { 0, 0 };
    UnityEvent onTurnChange = new UnityEvent();
    UnityEvent onScoreChange = new UnityEvent();

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
            tanks[0].animation = tank1.GetComponent<TankAnimation>();
            tanks[1].animation = tank2.GetComponent<TankAnimation>();

            tanks[0].movement.enabled = true;
            tanks[1].movement.enabled = false;
            tanks[0].shooting.enabled = true;
            tanks[1].shooting.enabled = false;
            tanks[0].animation.enabled = true;
            tanks[1].animation.enabled = false;

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
            if (!tank.movement.IsRespawning)
                tank.movement.enabled = !tank.movement.enabled;
            tank.shooting.enabled = !tank.shooting.enabled;
            tank.animation.enabled = !tank.animation.enabled;

            if (tank.shooting.enabled)
            {
                tank.shooting.ReEnableFiring();
                activeTank = tank;
            }
            else
                tank.animation.DisableMovementAnimation();
        }

        onTurnChange.Invoke();
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

    public int GetPlayerScore(int index)
    {
        return playerScores[index];
    }

    public float GetViewBound(ViewBound viewBound)
    {
        return viewBounds[viewBound];
    }

    public void IncreasePlayerScore(float speedAtShot)
    {
        Tank nonActiveTank = Array.Find(tanks, t => t.index != activeTank.index);
        
        nonActiveTank.animation.PlayReceiveFire();
        nonActiveTank.movement.Respawn();
        
        playerScores[activeTank.index] += (int)speedAtShot;
        onScoreChange.Invoke();
    }

    public UnityEvent OnTurnChange
    {
        get { return onTurnChange; }
    }

    public UnityEvent OnScoreChange
    {
        get { return onScoreChange; }
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