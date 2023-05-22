using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameclear : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Title");
        BMGcontroller.Instance.StartBGM();
    }
}
