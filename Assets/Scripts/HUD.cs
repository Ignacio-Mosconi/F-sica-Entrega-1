using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] string speedString = "Speed: ";
    [SerializeField] string angleString = "Angle: ";
    [SerializeField] TextMeshProUGUI projectileSpeedText;
    [SerializeField] TextMeshProUGUI aimingAngleText;
    [SerializeField] TankShooting[] tankShootings;

    void Start()
    {
        foreach (TankShooting tankShooting in tankShootings)
        {
            tankShooting.OnSpeedChange.AddListener(ChangeSpeedText);
            tankShooting.OnAngleChange.AddListener(ChangeAngleText);
        }
    }

    void ChangeSpeedText()
    {
        projectileSpeedText.text = speedString + (int)TurnManager.Instance.GetActiveTankShooting().CurrentProjectileSpeed + " m/s";
    }

    void ChangeAngleText()
    {
        aimingAngleText.text = angleString + (int)TurnManager.Instance.GetActiveTankShooting().CurrentAimingAngle + " d";        
    }
}