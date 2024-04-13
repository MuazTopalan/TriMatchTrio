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

    public static int SandblockEnabled { get; private set; }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName.StartsWith("Level"))
        {
            int levelNumber = int.Parse(sceneName.Replace("Level", ""));
            PlayerPrefs.SetInt("CurrentLevel", levelNumber);
            
            SandblockEnabled = 1; // Varsayılan olarak sandblock etkin olsun
            
            if (levelNumber < 3) // 3. seviyeden önce
            {
                SandblockEnabled = 0; // Sandblock devre dışı bırakılsın
            }
            
            PlayerPrefs.SetInt("SandblockEnabled", SandblockEnabled);
        }
}


    // void Start()
    // {
    //     int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
    //     PlayerPrefs.SetInt("CurrentLevel", levelNumber);
        
    //     int sandblockEnabled = 0; // Varsayılan olarak sandblock etkin olmasın
        
    //     if (levelNumber >= 3) // 3. seviyeden itibaren
    //     {
    //         sandblockEnabled = 1;
    //     }
        
    //     PlayerPrefs.SetInt("SandblockEnabled", sandblockEnabled);
    // }
}
