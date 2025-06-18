using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [System.Serializable]
    private struct LevelInfo
    {
        public string levelKey;
        public string sceneName;
        public Vector2 entryPosition;
    }

    private struct Level
    {
        public string sceneName;
        public Vector2 entryPosition;
    }

    [SerializeField] private List<LevelInfo> levelInformation;
    private Dictionary<string, Level> levelDictionary;

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Initialize()
    {
        levelDictionary = new Dictionary<string, Level>();

        foreach (var levelInfo in levelInformation)
        {
            Level newLevel = new()
            {
                sceneName = levelInfo.sceneName,
                entryPosition = levelInfo.entryPosition
            };
            levelDictionary[levelInfo.levelKey] = newLevel;
        }

        levelInformation = null;
    }

    public void LoadScene(string key)
    {
        if (levelDictionary.TryGetValue(key, out Level level))
        {
            Debug.Log("Loading " + level.sceneName);
            SceneManager.LoadSceneAsync(level.sceneName);
        }
        else
        {
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded");
    }
}
