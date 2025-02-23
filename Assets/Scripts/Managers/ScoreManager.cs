using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int initialScore;
    public int InitialScore{
        get{
            return initialScore;
        }
    }
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int _score;
    public int Score{
        get{
            return _score;
        }
    }
    
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

    public void AddScore(int point)
    {
        _score += point;
    }

    public void InitializeScore(){
        _score = initialScore;
    }
}
