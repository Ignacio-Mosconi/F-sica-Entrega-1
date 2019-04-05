﻿using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] projectileSpeedTexts;
    [SerializeField] TextMeshProUGUI[] aimingAngleTexts;

    void Start()
    {
        foreach (Tank tank in GameManager.Instance.Tanks)
        {
            projectileSpeedTexts[tank.index].text = tank.shooting.CurrentProjectileSpeed.ToString();
            aimingAngleTexts[tank.index].text = tank.shooting.CurrentAimingAngle.ToString();
            
            tank.shooting.OnSpeedChange.AddListener(ChangeSpeedText);
            tank.shooting.OnAngleChange.AddListener(ChangeAngleText);
        }
    }

    void ChangeSpeedText()
    {
        Tank activeTank = GameManager.Instance.GetActiveTank();
        TankShooting activeTankShooting = activeTank.shooting;
        int activeTankIndex = activeTank.index;
        int speedValue = (int)activeTankShooting.CurrentProjectileSpeed;

        projectileSpeedTexts[activeTankIndex].text = speedValue.ToString();
    }

    void ChangeAngleText()
    {
        Tank activeTank = GameManager.Instance.GetActiveTank();
        TankShooting activeTankShooting = activeTank.shooting;
        int activeTankIndex = activeTank.index;
        int angleValue = (int)activeTankShooting.CurrentAimingAngle;

        aimingAngleTexts[activeTankIndex].text = angleValue.ToString();       
    }
}