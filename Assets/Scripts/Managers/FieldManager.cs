using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance { get; private set; }
    [SerializeField] private int wave;
    [SerializeField] private List<FieldPattern> fieldPatterns; 
    [SerializeField] private int maxSamePatternCnt;
    private int samePatternCnt = 0;
    private string previousPattern;
    private string currentPattern;
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
        //* This code is executed when testing in PlayScene
        // StartGame()
    }

    public IEnumerator StartGame(){
        earlyPatternIndex = 0;

        //! Why I fucking need a loading time?
        //! If I don't put this code, The first executed pattern doens't work properly
        //! I don't know the fxxking reason. Maybe beacause of sequence shit?
        yield return new WaitForSeconds(0.5f); 

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

            currentPattern = ((PatternType)earlyPatternIndex).ToString();
            previousPattern = currentPattern;
            samePatternCnt = 0;
        }
        else{
            int randomIndex;
            do{
                randomIndex = Random.Range(0, fieldPatterns.Count);
                currentPattern = ((PatternType)randomIndex).ToString();

                if (currentPattern == previousPattern){
                    samePatternCnt++;
                }
                else{
                    samePatternCnt = 0;
                }
            }
            while(samePatternCnt >= maxSamePatternCnt);

            previousPattern = currentPattern;

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
