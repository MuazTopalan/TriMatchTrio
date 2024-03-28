using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBtnUI : MonoBehaviour
{
    public string MainMenuSceneName = "MenuScene";

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(2);
    }
}
