using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject registerPanel;
    public GameObject LoginPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenLoginPanel()
    {
        Debug.Log("LoginPanelopened");

        registerPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void OpenRegisterPanel()
    {
        Debug.Log("LoginPanelopened");

        registerPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }
}
