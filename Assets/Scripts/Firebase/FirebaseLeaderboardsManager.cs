using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseLeaderboardsManager : MonoBehaviour
{
    public static FirebaseLeaderboardsManager Instance;
    private DatabaseReference databaseReference;

    [SerializeField] private List<Transform> LeaderBoardsContent;
    [SerializeField] private GameObject LeaderBoardsElementObjectPrefab;

    [SerializeField] private int MaxDataLoadCount;

    [SerializeField] private TextMeshProUGUI PlayerNameText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        LoadScoreboardData();
    }

    public void LoadScoreboardData()
    {
        StartCoroutine(LoadScoreboardDataAsync());
    }

    private IEnumerator LoadScoreboardDataAsync()
    {
        string currentUserName = FirebaseRealtimeDataSaver.Instance.dataToSave.UserName;
        PlayerNameText.text = currentUserName;

        for (int i = 0; i < LeaderBoardsContent.Count; i++)
        {
            Task<DataSnapshot> databaseTask = databaseReference.Child("Users").OrderByChild("HighScore" + $"{i + 1}").GetValueAsync();

            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

            if (databaseTask.Exception != null)
            {
                Debug.LogWarning("LoadScoreboardDataAsync");
            }
            else
            {
                DataSnapshot snapshot = databaseTask.Result;

                if (LeaderBoardsContent[i].childCount > 0)
                {
                    foreach (Transform child in LeaderBoardsContent[i].transform)
                    {
                        Destroy(child.gameObject);
                    }
                }

                int currentDataLoadCount = 0;
                bool isCurrentUserInTop = false;

                foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
                {
                    if (currentDataLoadCount < MaxDataLoadCount)
                    {
                        string userName = childSnapshot.Child("UserName").Value.ToString();
                        string level = childSnapshot.Child("CurrentLevel").Value.ToString();
                        string highScore = childSnapshot.Child("HighScore" + $"{i + 1}").Value.ToString();
                        string userPlace = (currentDataLoadCount + 1).ToString();

                        GameObject GO = InstantiateLeaderBoardsElement(userName, highScore, level, userPlace, LeaderBoardsContent[i]);

                        currentDataLoadCount++;

                        if (currentUserName == userName)
                        {
                            isCurrentUserInTop = true;
                            GO.GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }

                if (!isCurrentUserInTop)
                {
                    int currentUserPlace = 0;
                    bool isCurrentUserFound = false;

                    foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
                    {
                        if (!isCurrentUserFound)
                        {
                            string userName = childSnapshot.Child("UserName").Value.ToString();
                            currentUserPlace++;

                            if (currentUserName == userName)
                            {
                                isCurrentUserFound = true;

                                string highScore = childSnapshot.Child("HighScore" + $"{i + 1}").Value.ToString();
                                string userLevel = FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel.ToString();
                                string userPlace = currentUserPlace.ToString();

                                GameObject GO = InstantiateLeaderBoardsElement(currentUserName, highScore, userLevel, userPlace, LeaderBoardsContent[i]);
                                GO.GetComponent<Image>().color = Color.yellow;
                            }
                        }
                    }
                }
            }
        }
    }

    public GameObject InstantiateLeaderBoardsElement(string userName, string userHighScore, string userLevel, string userPlace, Transform content )
    {
        GameObject leaderBoardsElementGO = Instantiate(LeaderBoardsElementObjectPrefab, content);
        LeaderBoardsElement leaderBoardsElement = leaderBoardsElementGO.GetComponent<LeaderBoardsElement>();
        leaderBoardsElement.NameText.text = userName;
        leaderBoardsElement.HighScoreText.text = userHighScore;
        leaderBoardsElement.LevelText.text = userLevel;
        leaderBoardsElement.PlaceText.text = "#" + userPlace;

        return leaderBoardsElementGO;
    }

    public void OpenLevelLeaderboards(int level)
    {
        for (int i = 0; i < LeaderBoardsContent.Count; i++)
        {
            LeaderBoardsContent[i].gameObject.SetActive(false);
            LeaderBoardsContent[level - 1].gameObject.SetActive(true);
        }
    }
}
