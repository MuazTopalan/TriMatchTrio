using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    public Sprite SoundOnSprite;
    public Sprite SoundOffSprite;
    public Image SoundButtonImage;

    public Sprite DevPanelOpenSprite;
    public Sprite DevPanelCloseSprite;
    public Image DevButtonImage;
    private bool _isDevAreaOpen = false;
    public GameObject DevPanel; 

    private SoundManager _soundManager;

    public TextMeshProUGUI PlayerNameText;

    private void Start()
    {
        _soundManager = SoundManager.Instance;

        PlayerNameText.text = FirebaseAuthManager.Instance.User.DisplayName;
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void OnLeaderBoardButtonClicked()
    {
        SceneManager.LoadScene(3);
    }

    public void OnLevelButtonClicked()
    {
        SceneManager.LoadScene(4);
    }

    public void OnContinueButtonClicked()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);

        FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel = FirebaseRealtimeDataSaver.Instance.GetLevelNumberFromCurrentSceneName() + 1;
        FirebaseRealtimeDataSaver.Instance.SaveData();
    }

    public void OnRetryButtonClicked()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();

        // Exit play mode
        EditorApplication.ExitPlaymode();
    }

    public void OnSoundButtonClicked()
    {
        _soundManager.ToggleSound();

        if (_soundManager.IsSoundOn())
        {
            SoundButtonImage.sprite = SoundOnSprite;
        }
        else
        {
            SoundButtonImage.sprite = SoundOffSprite;
        }
    }

    public void OnDevButtonClicked()
    {
        if (!_isDevAreaOpen)
        {
            DevButtonImage.sprite = DevPanelCloseSprite;
            DevPanel.SetActive(true);
            _isDevAreaOpen = true;
        }
        else
        {
            DevButtonImage.sprite = DevPanelOpenSprite;
            DevPanel.SetActive(false);
            _isDevAreaOpen = false;
        }
    }
}
