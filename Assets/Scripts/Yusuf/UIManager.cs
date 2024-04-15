using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject LoginPanel;

    public Button RegisterPanelButton;
    [SerializeField] private Button RegisterPanelBackButton;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        RegisterPanelButton.onClick.AddListener(OpenRegisterPanel);
        RegisterPanelBackButton.onClick.AddListener(OpenLoginPanel);
    }

    private void OnDisable()
    {
        RegisterPanelButton.onClick.RemoveListener(OpenRegisterPanel);
        RegisterPanelBackButton.onClick.RemoveListener(OpenLoginPanel);
    }

    public void OpenLoginPanel()
    {
        registerPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void OpenRegisterPanel()
    {
        registerPanel.SetActive(true);
        LoginPanel.SetActive(false);
    }
}
