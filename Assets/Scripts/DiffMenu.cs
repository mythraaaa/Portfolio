using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiffMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title");
    }

    public void GoToEasy()
    {
        BMGcontroller.Instance.StopBGM();
        SceneManager.LoadScene("Game");
    }

}
