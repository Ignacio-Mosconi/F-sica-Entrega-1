using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainScreen;
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject exitScreen;

    public void PlayGame()
    {
        GameManager.Instance.StartGame();    
        gameObject.SetActive(false);
    }

    public void ShowControls()
    {
        mainScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void ShowExit()
    {
        mainScreen.SetActive(false);
        exitScreen.SetActive(true);
    }

    public void ReturnToMainScreen()
    {
        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
        exitScreen.SetActive(false);
    }

    public void Exit()
    {
        GameManager.Instance.ExitApplication();
    }
}