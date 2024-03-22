using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerYusuf : MonoBehaviour
{
    public TextMeshProUGUI CurrentLevelText;
    public TextMeshProUGUI CurrentScoreText;
    public TextMeshProUGUI HighScoreText;

    public Button IncreaseLevelButton;
    public Button IncreaseScoreButton;

    private const string c_currentLevel = "Current Level: ";
    private const string c_currentScore = "Current Score: ";
    private const string c_highScore = "High Score: ";

    public int CurrentLevel;
    public int CurrentScore;
    public int HighScore;

    private void OnEnable()
    {
        IncreaseLevelButton.onClick.AddListener(IncreaseLevel);
        IncreaseScoreButton.onClick.AddListener(IncreaseScore);
    }

    private void OnDisable()
    {
        IncreaseLevelButton.onClick.RemoveListener(IncreaseLevel);
        IncreaseScoreButton.onClick.RemoveListener(IncreaseScore);
    }

    private void Start()
    {
        LoadData();
    }

    public void IncreaseLevel()
    {
        CurrentLevel++;
        FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel = CurrentLevel;
        SaveData();
        CurrentLevelText.text = c_currentLevel + CurrentLevel;
    }

    public void IncreaseScore()
    {
        CurrentScore++;
        CurrentScoreText.text = c_currentScore + CurrentScore;
        IncreaseHighScore();
    }

    public void IncreaseHighScore()
    {
        if (CurrentScore > HighScore)
        { 
            HighScore = CurrentScore;
            HighScoreText.text = c_highScore + HighScore;
            FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore = HighScore;
            SaveData();
        }
    }

    private void SaveData()
    {
        FirebaseRealtimeDataSaver.Instance.SaveData();
    }

    private void LoadData()
    {
        StartCoroutine(LoadDataAsync());
    }

    private IEnumerator LoadDataAsync()
    {
        if(FirebaseRealtimeDataSaver.Instance.isDataLoaded == false)
        {
            yield return new WaitForEndOfFrame();
            StartCoroutine(LoadDataAsync());
        }
        else
        {
            CurrentLevel = FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel;
            HighScore = FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore;
            CurrentLevelText.text = c_currentLevel + CurrentLevel;
            HighScoreText.text += c_highScore + HighScore;
        }
    }

}
