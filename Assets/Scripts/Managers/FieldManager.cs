using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance { get; private set; }

    [SerializeField] private int wave;
    [SerializeField] private List<FieldPattern> fieldPatterns; 
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

    private void Start() {
        StartCoroutine(StartWave());
    }

    public IEnumerator StartWave()
    {
        Debug.Log("New Wave Started!");
        wave++;

        int randomIndex = Random.Range(0, fieldPatterns.Count);
        FieldPattern fieldPattern = fieldPatterns[randomIndex];
        yield return StartCoroutine(fieldPattern.ExecutePattern());
        
        StartCoroutine(StartWave());
    }

    public void ExecuteGameOver(){
        StopAllCoroutines();
        wave = 0;
    }
}

public enum PatternType{
    TestPattern,
    HomingBulletPattern,
    EagleTroopPattern
}