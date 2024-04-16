using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseAnalyticsManager : MonoBehaviour
{
    public static FirebaseAnalyticsManager Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
        });
    }

    public void SendLevelCompletedEvent(int level, int totalMoves, float spendTimeInSeconds)
    {
        Parameter levelParameter = new("level_parameter", level);
        Parameter totalMovesParameter = new("total_moves_parameter", totalMoves);
        Parameter spendTimeInSecondsParameter = new("spend_time_in_seconds", spendTimeInSeconds);

        Parameter[] parameters = { levelParameter, totalMovesParameter, spendTimeInSecondsParameter };

        FirebaseAnalytics.LogEvent("level_completed", parameters);
    }

    public void SendLevelFailedEvent(int level, int totalMoves, float spendTimeInSeconds)
    {
        Parameter levelParameter = new("level_parameter", level);
        Parameter totalMovesParameter = new("total_moves_parameter", totalMoves);
        Parameter spendTimeInSecondsParameter = new("spend_time_in_seconds", spendTimeInSeconds);

        Parameter[] parameters = { levelParameter, totalMovesParameter, spendTimeInSecondsParameter };

        FirebaseAnalytics.LogEvent("level_failed", parameters);
    }
}
