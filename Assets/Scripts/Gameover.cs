using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title");
        BMGcontroller.Instance.StartBGM();
        Time.timeScale = 1;
    }

    public void Continue()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }
}
