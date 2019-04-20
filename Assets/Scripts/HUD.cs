using System.Collections;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] RectTransform[] uiPanels;
    [SerializeField] GameObject[] activePlayerPanels;
    [SerializeField] TextMeshProUGUI[] projectileSpeedTexts;
    [SerializeField] TextMeshProUGUI[] aimingAngleTexts;
    [SerializeField] TextMeshProUGUI[] speedTitleTexts;
    [SerializeField] TextMeshProUGUI[] angleTitleTexts;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Color activeModifierColor;
    [SerializeField] Color inactiveModifierColor;
    [SerializeField] [Range(0.1f, 1f)] float panelScaleDuration = 0.75f;
    [SerializeField] [Range(1f, 1.5f)] float panelEnlargedScale = 1.2f;
    [SerializeField] [Range(0.5f, 1f)] float panelShrinkedScale = 0.8f;

    void Start()
    {
        foreach (Tank tank in GameManager.Instance.Tanks)
        {
            projectileSpeedTexts[tank.index].text = tank.shooting.CurrentProjectileSpeed.ToString();
            aimingAngleTexts[tank.index].text = tank.shooting.CurrentAimingAngle.ToString();
            
            GameManager.Instance.OnTurnChange.AddListener(ChangeActivePlayerUI);
            GameManager.Instance.OnScoreChange.AddListener(ChangeScoreText);
            tank.shooting.OnSpeedChange.AddListener(ChangeSpeedText);
            tank.shooting.OnAngleChange.AddListener(ChangeAngleText);
            tank.shooting.OnSpeedModifierPressed.AddListener(SpeedTextColorModifierOnPressed);
            tank.shooting.OnSpeedModifierReleased.AddListener(SpeedTextColorModifierOnRelease);
            tank.shooting.OnAngleModifierPressed.AddListener(AngleTextColorModifierOnPressed);
            tank.shooting.OnAngleModifierReleased.AddListener(AngleTextColorModifierOnRelease);
        }
    }

    void OnEnable()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;
        Tank nonActiveTank = GameManager.Instance.NonActiveTank;

        TankShooting activeTankShooting = activeTank.shooting;
        TankShooting nonActiveTankShooting = nonActiveTank.shooting;

        int activeTankIndex = activeTank.index;
        int nonActiveTankIndex = nonActiveTank.index;
        
        int activeTankSpeedValue = (int)activeTankShooting.CurrentProjectileSpeed;
        int nonActiveTankSpeedValue = (int)nonActiveTankShooting.CurrentProjectileSpeed;

        int activeTankAngleValue = (int)activeTankShooting.CurrentAimingAngle;
        int nonActiveTankAngleValue = (int)nonActiveTankShooting.CurrentAimingAngle;

        projectileSpeedTexts[activeTankIndex].text = activeTankSpeedValue.ToString();
        projectileSpeedTexts[nonActiveTankIndex].text = nonActiveTankSpeedValue.ToString();
        
        aimingAngleTexts[activeTankIndex].text = activeTankAngleValue.ToString();
        aimingAngleTexts[nonActiveTankIndex].text = nonActiveTankAngleValue.ToString();

        scoreTexts[activeTank.index].text = GameManager.Instance.GetPlayerScore(activeTank.index).ToString();
        scoreTexts[nonActiveTank.index].text = GameManager.Instance.GetPlayerScore(nonActiveTank.index).ToString();

        ChangeActivePlayerUI();
    }

    void Update()
    {
        timeText.text = ((int)(GameManager.Instance.TurnTime)).ToString();
    }

    void ChangeSpeedText()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;
        TankShooting activeTankShooting = activeTank.shooting;
        int activeTankIndex = activeTank.index;
        int speedValue = (int)activeTankShooting.CurrentProjectileSpeed;

        projectileSpeedTexts[activeTankIndex].text = speedValue.ToString();
    }

    void ChangeAngleText()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;
        TankShooting activeTankShooting = activeTank.shooting;
        int activeTankIndex = activeTank.index;
        int angleValue = (int)activeTankShooting.CurrentAimingAngle;

        aimingAngleTexts[activeTankIndex].text = angleValue.ToString();       
    }

    void ChangeActivePlayerUI()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;
        Tank nonActiveTank = GameManager.Instance.NonActiveTank;

        StartCoroutine(ScaleUIPanel(uiPanels[activeTank.index], enlarge: true));
        StartCoroutine(ScaleUIPanel(uiPanels[nonActiveTank.index], enlarge: false));

        activePlayerPanels[activeTank.index].SetActive(true);
        activePlayerPanels[nonActiveTank.index].SetActive(false);

        speedTitleTexts[nonActiveTank.index].color = inactiveModifierColor;
        angleTitleTexts[nonActiveTank.index].color = inactiveModifierColor;

        bool highilightSpeed = (Input.GetButton("Adjust Speed Modifier")) && (!Input.GetButton("Adjust Angle Modifier"));
        bool highlightAngle = (Input.GetButton("Adjust Angle Modifier")) && (!Input.GetButton("Adjust Speed Modifier"));
        
        speedTitleTexts[activeTank.index].color = (highilightSpeed) ? activeModifierColor : inactiveModifierColor;
        angleTitleTexts[activeTank.index].color = (highlightAngle) ? activeModifierColor : inactiveModifierColor;
    }

    IEnumerator ScaleUIPanel(RectTransform panel, bool enlarge)
    {
        float time = 0f;
        Vector3 initialScale = panel.localScale;
        Vector3 targetScale = (enlarge) ? new Vector3(panelEnlargedScale, panelEnlargedScale, panelEnlargedScale) : 
                                            new Vector3(panelShrinkedScale, panelShrinkedScale, panelShrinkedScale);

        while (panel.localScale != targetScale)
        {
            time += Time.deltaTime;
            panel.localScale = Vector3.Lerp(initialScale, targetScale, time / panelScaleDuration);

            yield return null;
        }
    }

    void SpeedTextColorModifierOnPressed()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;

        speedTitleTexts[activeTank.index].color = activeModifierColor;
        angleTitleTexts[activeTank.index].color = inactiveModifierColor;
    }

    void SpeedTextColorModifierOnRelease()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;

        speedTitleTexts[activeTank.index].color = inactiveModifierColor;

        if (Input.GetButton("Adjust Angle Modifier"))
            angleTitleTexts[activeTank.index].color = activeModifierColor;
    }

    void AngleTextColorModifierOnPressed()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;

        angleTitleTexts[activeTank.index].color = activeModifierColor;
        speedTitleTexts[activeTank.index].color = inactiveModifierColor;
    }

    void AngleTextColorModifierOnRelease()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;

        angleTitleTexts[activeTank.index].color = inactiveModifierColor;

        if (Input.GetButton("Adjust Speed Modifier"))
            speedTitleTexts[activeTank.index].color = activeModifierColor;
    }

    void ChangeScoreText()
    {
        Tank activeTank = GameManager.Instance.ActiveTank;

        scoreTexts[activeTank.index].text = GameManager.Instance.GetPlayerScore(activeTank.index).ToString();
    }
}