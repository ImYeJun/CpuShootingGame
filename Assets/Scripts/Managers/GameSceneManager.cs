using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the instance across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(SceneType type){
        SceneManager.LoadScene((int)type);

        switch (type){
            case SceneType.PlayScene:
                StartCoroutine(FieldManager.Instance.StartGame());
                ScoreManager.Instance.InitializeScore();
                BgmManager.Instance.SetBgm(BgmType.PlaySceneBgm);
                //* Player Health is initialized in PlayerHeathManager.
                break;

            case SceneType.GameOverScene:
                BgmManager.Instance.SetBgm(BgmType.GameOverBgm);
                FieldManager.Instance.ExecuteGameOver();
                break;

            default:
                break;
        }
    }
}

public enum SceneType{
    StartScene,
    PlayScene,
    GameOverScene
}
