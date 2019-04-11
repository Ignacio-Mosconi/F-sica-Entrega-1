using System.Collections;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] RectTransform[] uiPanels;
    [SerializeField] TextMeshProUGUI[] activePlayerTexts;
    [SerializeField] TextMeshProUGUI[] projectileSpeedTexts;
    [SerializeField] TextMeshProUGUI[] aimingAngleTexts;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float panelScaleDuration = 2f;
    [SerializeField] float panelEnlargedScale = 2f;

    void Start()
    {
        foreach (Tank tank in GameManager.Instance.Tanks)
        {
            projectileSpeedTexts[tank.index].text = tank.shooting.CurrentProjectileSpeed.ToString();
            aimingAngleTexts[tank.index].text = tank.shooting.CurrentAimingAngle.ToString();
            
            GameManager.Instance.OnTurnChange.AddListener(ChangeActivePlayerUI);
            tank.shooting.OnSpeedChange.AddListener(ChangeSpeedText);
            tank.shooting.OnAngleChange.AddListener(ChangeAngleText);
        }

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
        Tank[] tanks = GameManager.Instance.Tanks;
        Tank activeTank = GameManager.Instance.ActiveTank;
        Tank nonActiveTank = tanks[0];

        foreach (Tank tank in tanks)
            if (tank.index != activeTank.index)
                nonActiveTank = tank;

        StartCoroutine(ScaleUIPanel(uiPanels[activeTank.index], true));
        StartCoroutine(ScaleUIPanel(uiPanels[nonActiveTank.index], false));

        activePlayerTexts[activeTank.index].gameObject.SetActive(true);
        activePlayerTexts[nonActiveTank.index].gameObject.SetActive(false);
    }

    IEnumerator ScaleUIPanel(RectTransform panel, bool enlarge)
    {
        float time = 0f;
        Vector3 initialScale = panel.localScale;
        Vector3 targetScale = (enlarge) ? new Vector3(panelEnlargedScale, panelEnlargedScale, panelEnlargedScale) : 
                                            new Vector3(1f, 1f, 1f);

        while (panel.localScale != targetScale)
        {
            time += Time.deltaTime;
            panel.localScale = Vector3.Lerp(initialScale, targetScale, time / panelScaleDuration);

            yield return null;
        }
    }
}