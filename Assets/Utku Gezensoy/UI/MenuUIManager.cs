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

    void Start()
    {
        _soundManager = SoundManager.Instance;

        PlayerNameText.text = FirebaseAuthManager.Instance.User.DisplayName;

        GameObject buttonPanel = GameObject.Find("ButtonPanel");

        if (buttonPanel != null)
        {
            Button[] buttons = buttonPanel.GetComponentsInChildren<Button>();

            foreach (Button button in buttons)
            {
                if (button.name != "SoundBtn" && button.name != "ExitBtn")
                {
                    button.onClick.AddListener(() => OnButtonClicked());
                }
            }
        }
    }

    public void OnButtonClicked()
    {
        _soundManager.PlayBtnSfx();
    }

    public void OnStartButtonClicked()
    {


        if (FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel == 0)
        {
            SceneManager.LoadScene("Level1");
        }
        else if (FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel > 5)
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            SceneManager.LoadScene("Level" + $"{FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel}");

        }
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
        //int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        //SceneManager.LoadScene(nextSceneIndex);

        int nextSceneIndex = FirebaseRealtimeDataSaver.Instance.GetLevelNumberFromCurrentSceneName() + 1;
        if (nextSceneIndex > 5)
        {
            nextSceneIndex = 1;
        }
        SceneManager.LoadScene("Level" + $"{nextSceneIndex}");
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
