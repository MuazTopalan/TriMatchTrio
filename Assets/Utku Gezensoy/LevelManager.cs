using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("LevelManagerSingleton");
                    _instance = singletonObject.AddComponent<LevelManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
        PlayerPrefs.SetInt("CurrentLevel", levelNumber);
        
        int sandblockEnabled = 1; // Varsayılan olarak sandblock etkin olsun
        switch (levelNumber)
        {
            case 1:
            case 2:
                sandblockEnabled = 0; // 1. ve 2. seviyelerde sandblock olmayacak
                break;
        }
        PlayerPrefs.SetInt("SandblockEnabled", sandblockEnabled);
    }
}
