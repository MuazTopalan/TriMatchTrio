using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        _soundManager = SoundManager.Instance;
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
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
