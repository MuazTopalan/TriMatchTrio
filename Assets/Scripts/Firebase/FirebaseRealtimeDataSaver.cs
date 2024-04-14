using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DataToSave
{
    public string UserName;
    public int CurrentLevel;
    public int HighScore;
}

public class FirebaseRealtimeDataSaver : MonoBehaviour
{
    public static FirebaseRealtimeDataSaver Instance;

    public DataToSave dataToSave;
    public string UserID;
    private DatabaseReference databaseReference;

    public bool isDataLoaded;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(dataToSave);
        databaseReference.Child("Users").Child(UserID).SetRawJsonValueAsync(json);
    }

    public void LoadData()
    {
        StartCoroutine(LoadDataAsync());
    }

    private IEnumerator LoadDataAsync()
    {
        var serverData = databaseReference.Child("Users").Child(UserID).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        DataSnapshot dataSnapshot = serverData.Result;
        string jsonData = dataSnapshot.GetRawJsonValue();

        if(jsonData != null)
        {
            dataToSave = JsonUtility.FromJson<DataToSave>(jsonData);
            isDataLoaded = true;
        }
        else
        {

        }
    }

    public int GetLevelNumberFromCurrentSceneName()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        string emptyString = string.Empty;
        int LevelNumber = 0;

        for (int i = 0; i < SceneName.Length; i++)
        {
            if (char.IsDigit(SceneName[i]))
                emptyString += SceneName[i];
        }

        if (emptyString.Length > 0)
            LevelNumber = int.Parse(emptyString);

        return LevelNumber;
    }
}
