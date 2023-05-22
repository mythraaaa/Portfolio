using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title");
    }
   
}
