using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseLeaderboardsManager : MonoBehaviour
{
    public static FirebaseLeaderboardsManager instance;
    private DatabaseReference databaseReference;

    public Transform LeaderBoardsContent;
    public GameObject LeaderBoardsElementObjectPrefab;

    public int MaxDataLoadCount;

    public Button RefreshButton;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void OnEnable()
    {
        RefreshButton.onClick.AddListener(LoadScoreboardData);
    }

    private void OnDisable()
    {
        RefreshButton.onClick.RemoveListener(LoadScoreboardData);
    }

    public void LoadScoreboardData()
    {
        StartCoroutine(LoadScoreboardDataAsync());
    }

    private IEnumerator LoadScoreboardDataAsync()
    {

        Task<DataSnapshot> databaseTask = databaseReference.Child("Users").OrderByChild("HighScore").GetValueAsync();

        yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);

        if(databaseTask.Exception != null)
        {
            Debug.LogWarning("herryk");
        }
        else
        {
            DataSnapshot snapshot = databaseTask.Result;

            if(LeaderBoardsContent.childCount > 0)
            {
                foreach (Transform child in LeaderBoardsContent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            int currentDataLoadCount = 0;

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
            {
                if (currentDataLoadCount < MaxDataLoadCount)
                {
                    string userName = childSnapshot.Child("UserName").Value.ToString();
                    string level = childSnapshot.Child("CurrentLevel").Value.ToString();
                    string highScore = childSnapshot.Child("HighScore").Value.ToString();

                    GameObject leaderBoardsElementGO = Instantiate(LeaderBoardsElementObjectPrefab, LeaderBoardsContent);
                    LeaderBoardsElement leaderBoardsElement = leaderBoardsElementGO.GetComponent<LeaderBoardsElement>();
                    leaderBoardsElement.PlaceText.text = "#" + (currentDataLoadCount + 1);
                    leaderBoardsElement.NameText.text = userName;
                    leaderBoardsElement.LevelText.text = level;
                    leaderBoardsElement.HighScoreText.text = highScore;

                    currentDataLoadCount++;
                }
                else
                {
                    string currentUserName = FirebaseRealtimeDataSaver.Instance.dataToSave.UserName;
                    bool userInTop = false;

                    for (int i = 0; i < MaxDataLoadCount; i++)
                    {
                        if (currentUserName == LeaderBoardsContent.GetChild(i).GetComponent<LeaderBoardsElement>().NameText.text)
                        {
                            userInTop = true;
                        }
                        
                    }

                    if (!userInTop)
                    {
                        int userHighScore = FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore;

                        GameObject leaderBoardsElementGO = Instantiate(LeaderBoardsElementObjectPrefab, LeaderBoardsContent);
                        LeaderBoardsElement leaderBoardsElement = leaderBoardsElementGO.GetComponent<LeaderBoardsElement>();
                        leaderBoardsElement.NameText.text = currentUserName;
                        leaderBoardsElement.HighScoreText.text = userHighScore.ToString();
                    }
                }
                
            }
        }

    }
}
