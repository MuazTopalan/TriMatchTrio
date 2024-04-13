using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    public TextMeshProUGUI CurrentUserNameText;
    public GameObject LeaderBoardsPanel;
    public Button LeaderBoardsPanelButton;
    public Button LeaderBoardsPanelBackButton;
    public Button LeaderBoardsPanelRefreshButton;

    private void OnEnable()
    {
        LeaderBoardsPanelButton.onClick.AddListener(OpenLeaderBoardsPanel);
        LeaderBoardsPanelBackButton.onClick.AddListener(CloseLeaderBoardsPanel);
        LeaderBoardsPanelRefreshButton.onClick.AddListener(RefreshLeaderBoards);
    }

    private void OnDisable()
    {
        LeaderBoardsPanelButton.onClick.RemoveListener(OpenLeaderBoardsPanel);
        LeaderBoardsPanelBackButton.onClick.RemoveListener(CloseLeaderBoardsPanel);
        LeaderBoardsPanelRefreshButton.onClick.RemoveListener(RefreshLeaderBoards);
    }

    private void Start()
    {
        CurrentUserNameText.text = FirebaseAuthManager.Instance.User.DisplayName;
    }

    public void OpenLeaderBoardsPanel()
    {
        LeaderBoardsPanel.SetActive(true);
        FirebaseLeaderboardsManager.Instance.LoadScoreboardData();
    }

    public void CloseLeaderBoardsPanel()
    {
        LeaderBoardsPanel.SetActive(false);
    }
    
    public void RefreshLeaderBoards()
    {
        FirebaseLeaderboardsManager.Instance.LoadScoreboardData();
    }
}
