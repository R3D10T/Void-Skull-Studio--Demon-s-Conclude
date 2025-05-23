using LP.TurnBased;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingMenu;

    public string sceneName = "MainMenu";


    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);

        Time.timeScale = 1;
        isPaused = false;

    }

    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Tutorial Battle");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);

        Time.timeScale = 0;
        isPaused = true;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadIntoMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);
        SceneManager.LoadScene(sceneName);

    }
}
