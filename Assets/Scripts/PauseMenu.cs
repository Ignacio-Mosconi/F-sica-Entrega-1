using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject mainScreen;

    bool isPaused = false;

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    void Pause()
    {
        isPaused = true;
        mainScreen.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.SetPauseState(paused: true);
    }

    public void Resume()
    {
        isPaused = false;
        mainScreen.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.SetPauseState(paused: false);
    }

    public void Exit()
    {
        isPaused = false;
        mainScreen.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.ExitGame();
    }
}