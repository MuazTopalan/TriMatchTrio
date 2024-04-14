using Firebase.Database;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseLeaderboardsManager : MonoBehaviour
{
    public static FirebaseLeaderboardsManager Instance;
    private DatabaseReference databaseReference;

    public Transform LeaderBoardsContent;
    public GameObject LeaderBoardsElementObjectPrefab;

    public int MaxDataLoadCount;

    public Button RefreshButton;

    public TextMeshProUGUI PlayerNameText;

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
            bool isCurrentUserInTop = false;
            string currentUserName = FirebaseRealtimeDataSaver.Instance.dataToSave.UserName;

            PlayerNameText.text = currentUserName;


            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
            {
                if (currentDataLoadCount < MaxDataLoadCount)
                {
                    string userName = childSnapshot.Child("UserName").Value.ToString();
                    string level = childSnapshot.Child("CurrentLevel").Value.ToString();
                    string highScore = childSnapshot.Child("HighScore").Value.ToString();
                    string userPlace = (currentDataLoadCount + 1).ToString();

                    GameObject GO = InstantiateLeaderBoardsElement(userName, highScore, level, userPlace);

                    currentDataLoadCount++;

                    if(currentUserName == userName)
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

                        if (currentUserName == userName) { isCurrentUserFound = true; }
                    }
                }

                string userHighScore = FirebaseRealtimeDataSaver.Instance.dataToSave.HighScore.ToString();
                string userLevel = FirebaseRealtimeDataSaver.Instance.dataToSave.CurrentLevel.ToString();
                string userPlace = currentUserPlace.ToString();

                GameObject GO = InstantiateLeaderBoardsElement(currentUserName, userHighScore, userLevel, userPlace);
                GO.GetComponent<Image>().color = Color.yellow;

            }
        }



    }

    public GameObject InstantiateLeaderBoardsElement(string userName, string userHighScore, string userLevel, string userPlace )
    {
        GameObject leaderBoardsElementGO = Instantiate(LeaderBoardsElementObjectPrefab, LeaderBoardsContent);
        LeaderBoardsElement leaderBoardsElement = leaderBoardsElementGO.GetComponent<LeaderBoardsElement>();
        leaderBoardsElement.NameText.text = userName;
        leaderBoardsElement.HighScoreText.text = userHighScore;
        leaderBoardsElement.LevelText.text = userLevel;
        leaderBoardsElement.PlaceText.text = "#" + userPlace;

        return leaderBoardsElementGO;
    }
}
