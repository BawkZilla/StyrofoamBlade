using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{

    TMP_Text _enemyLeftText;
    int _enemyCount = 0;

    [Header("Scene Settings")]
    public int StageIdx = 0;

    public static StageManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        onSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void Update()
    {
        if (_enemyCount > 0) return;

        if (Input.GetKeyDown(KeyCode.H))
        {
            MoveNextStage();
        }
    }

    void MoveNextStage()
    {
        if (SceneManager.GetActiveScene().name == "Boss") return;

        StageIdx += 1;
        SceneManager.LoadScene(StageIdx);
    }


    void onSceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        _enemyLeftText = GameObject.Find("EnemyCount").GetComponent<TMP_Text>();
        _enemyCount = FindObjectsByType<EnemyDataManager>(sortMode: FindObjectsSortMode.None).Length;
        SyncEnemyCountText();
    }

    public void OnEnemyDied()
    {
        _enemyCount -= 1;
        SyncEnemyCountText();
    }

    void SyncEnemyCountText()
    {
        _enemyLeftText.text = _enemyCount > 0 ? $"Enemy Left : {_enemyCount}" : "Press H to move next stage";
    }
}
