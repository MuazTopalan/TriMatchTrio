//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class GameManagerYusuf : MonoBehaviour
//{
//    public static GameManagerYusuf Instance;

//    public TextMeshProUGUI CurrentLevelText;
//    public TextMeshProUGUI CurrentScoreText;
//    public TextMeshProUGUI HighScoreText;
//    public TextMeshProUGUI MovesInCurrentLevelText;
//    public TextMeshProUGUI TimeSpendInCurrentLevel;

//    public Button IncreaseLevelButton;
//    public Button IncreaseScoreButton;
//    public Button MakeMoveButton;
//    public Button FinishLevelButton;
//    public Button QuitLevelButton;

//    private const string c_currentLevel = "Current Level: ";
//    private const string c_currentScore = "Current Score: ";
//    private const string c_highScore = "High Score: ";

//    public int CurrentLevel;
//    public int CurrentScore;
//    public int HighScore;

//    public int MovesInCurrentLevel;
//    public float spendSecondsCurrentLevel;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    private void OnEnable()
//    {
//        IncreaseLevelButton.onClick.AddListener(IncreaseLevel);
//        IncreaseScoreButton.onClick.AddListener(IncreaseScore);
//        MakeMoveButton.onClick.AddListener(MakeMove);
//        FinishLevelButton.onClick.AddListener(FinishLevel);
//        QuitLevelButton.onClick.AddListener(QuitLevel);
//    }

//    private void OnDisable()
//    {
//        IncreaseLevelButton.onClick.RemoveListener(IncreaseLevel);
//        IncreaseScoreButton.onClick.RemoveListener(IncreaseScore);
//        MakeMoveButton.onClick.RemoveListener(MakeMove);
//        QuitLevelButton.onClick.RemoveListener(QuitLevel);
//    }

//    private void Start()
//    {
//        LoadData();
//    }

//    private void Update()
//    {
//        spendSecondsCurrentLevel += Time.deltaTime;
//        TimeSpendInCurrentLevel.text = spendSecondsCurrentLevel.ToString("F2");


//    }

//    public void IncreaseLevel()
//    {
//        CurrentLevel++;
//        FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel = CurrentLevel;
//        SaveData();
//        CurrentLevelText.text = c_currentLevel + CurrentLevel;
//    }

//    public void IncreaseScore()
//    {
//        CurrentScore++;
//        CurrentScoreText.text = c_currentScore + CurrentScore;
//        IncreaseHighScore();
//    }

//    public void IncreaseHighScore()
//    {
//        if (CurrentScore > HighScore)
//        { 
//            HighScore = CurrentScore;
//            HighScoreText.text = c_highScore + HighScore;
//            FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore = HighScore;
//            SaveData();
//        }
//    }

//    public void MakeMove()
//    {
//        MovesInCurrentLevel++;
//        MovesInCurrentLevelText.text = "Moves In Current Level: " + MovesInCurrentLevel.ToString();
//    }

//    public void StartLevel(int level)
//    {
//        CurrentLevel = level;
//        CurrentLevelText.text = CurrentLevel.ToString();
//        spendSecondsCurrentLevel = 0;
//        MovesInCurrentLevel = 0;
//    }

//    public void FinishLevel()
//    {
//        FirebaseAnalyticsManager.Instance.SendLevelCompletedEvent(CurrentLevel, MovesInCurrentLevel, spendSecondsCurrentLevel);
//    }

//    public void QuitLevel()
//    {
//        FirebaseAnalyticsManager.Instance.SendLevelQuitedEvent(CurrentLevel, MovesInCurrentLevel, spendSecondsCurrentLevel);
//    }

//    private void SaveData()
//    {
//        FirebaseRealtimeDataSaver.Instance.dataToSave.UserName = FirebaseAuthManager.Instance.User.DisplayName;
//        FirebaseRealtimeDataSaver.Instance.SaveData();
//    }

//    private void LoadData()
//    {
//        StartCoroutine(LoadDataAsync());
//    }

//    private IEnumerator LoadDataAsync()
//    {
//        if(FirebaseRealtimeDataSaver.Instance.isDataLoaded == false)
//        {
//            yield return new WaitForEndOfFrame();
//            StartCoroutine(LoadDataAsync());
//        }
//        else
//        {
//            CurrentLevel = FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel;
//            HighScore = FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore;
//            CurrentLevelText.text = c_currentLevel + CurrentLevel;
//            HighScoreText.text += c_highScore + HighScore;
//        }
//    }

//}
