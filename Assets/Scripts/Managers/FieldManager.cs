using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance { get; private set; }
    [SerializeField] private int wave;
    [SerializeField] private List<FieldPattern> fieldPatterns; 
    private int earlyPatternIndex;
    
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
        earlyPatternIndex = 0;

        StartCoroutine(StartWave());
    }

    public IEnumerator StartWave()
    {
        wave++;
        Debug.Log("New Wave Started!");
        FieldPattern fieldPattern;

        if (fieldPatterns.Count <= 0){
            yield break;
        }

        if (wave <= fieldPatterns.Count){
            fieldPattern = fieldPatterns[earlyPatternIndex];

            yield return StartCoroutine(fieldPattern.ExecutePattern());

            earlyPatternIndex++;
            earlyPatternIndex %= fieldPatterns.Count;
        }
        else{
            int randomIndex = Random.Range(0, fieldPatterns.Count);
            fieldPattern = fieldPatterns[randomIndex];

            yield return StartCoroutine(fieldPattern.ExecutePattern(wave));
        }

        
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
