using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject MainPanel;
    public GameObject registerPanel;
    public GameObject LoginPanel;
    public GameObject LeaderBoardsPanel;

    public Button RegisterPanelButton;
    public Button LoginPanelButton;
    public Button LeaderBoardsPanelButton;

    public Button RegisterPanelBackButton;
    public Button LoginPanelBackButton;
    public Button LeaderBoardsPanelBackButton;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        RegisterPanelButton.onClick.AddListener(OpenRegisterPanel);
        LoginPanelButton.onClick.AddListener(OpenLoginPanel);
        RegisterPanelBackButton.onClick.AddListener(OpenMainPanel);
        LoginPanelBackButton.onClick.AddListener(OpenMainPanel);
        LeaderBoardsPanelButton.onClick.AddListener(OpenLeaderBoardsPanel);
        LeaderBoardsPanelBackButton.onClick.AddListener(OpenMainPanel);
    }

    private void OnDisable()
    {
        RegisterPanelButton.onClick.RemoveListener(OpenRegisterPanel);
        LoginPanelButton.onClick.RemoveListener(OpenLoginPanel);
        RegisterPanelBackButton.onClick.RemoveListener(OpenMainPanel);
        LoginPanelBackButton.onClick.RemoveListener(OpenMainPanel);
        LeaderBoardsPanelButton.onClick.RemoveListener(OpenLeaderBoardsPanel);
        LeaderBoardsPanelBackButton.onClick.RemoveListener(OpenMainPanel);
    }

    public void OpenLoginPanel()
    {
        MainPanel.SetActive(false);
        registerPanel.SetActive(false);
        LoginPanel.SetActive(true);
        LeaderBoardsPanel.SetActive(false);
    }

    public void OpenRegisterPanel()
    {
        MainPanel.SetActive(false);
        registerPanel.SetActive(true);
        LoginPanel.SetActive(false);
        LeaderBoardsPanel.SetActive(false);
    }

    public void OpenMainPanel()
    {
        //MainPanel.SetActive(true);
        registerPanel.SetActive(false);
        LoginPanel.SetActive(true);
        LeaderBoardsPanel.SetActive(false);
    }

    public void OpenLeaderBoardsPanel()
    {
        MainPanel.SetActive(false);
        registerPanel.SetActive(false);
        LoginPanel.SetActive(false);
        LeaderBoardsPanel.SetActive(true);

        FirebaseLeaderboardsManager.Instance.LoadScoreboardData();
    }
}
