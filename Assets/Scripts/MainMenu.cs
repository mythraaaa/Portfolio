using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
    public void SelectDiff()
    {
        SceneManager.LoadScene("Diff");
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("game");
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
