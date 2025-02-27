using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TestPattern : FieldPattern
{
    [Space(10)]
    [Header("TestPattern Field")]
    [SerializeField] private float enemyAreaRadius;
    [Header("WaveDifficultyPoint")]
    public List<WaveDifficultyPoint> min;
    public List<WaveDifficultyPoint> max;
    public override IEnumerator ExecutePattern(int wave = -1)
    {   
        List<GameObject> enemies = new List<GameObject>();
        CalculateActualWaveVariable(wave);
        Debug.Log("TestPattern Executed");

        int spawnEnemyCnt = Random.Range(actualMinSpawnEnemyCnt, actualMaxSpawnEnemyCnt);

        float minSpawnPositionX = cameraTopLeftCoord.x + 0.3f;
        float maxSpawnPositionX = cameraBottomRightCoord.x - 0.3f;

        float minSpawnPositionY = cameraTopLeftCoord.y + 1.0f;
        float maxSpawnPositionY = cameraTopLeftCoord.y + 4.0f;

        List<Vector3> occupiedPositions = new List<Vector3>();

        for (int i = 0; i < spawnEnemyCnt; i++)
        {
            Vector3 spawnPosition;
            int attempts = 0;
            bool positionFound = false;

            do
            {
                float spawnPointX = Random.Range(minSpawnPositionX, maxSpawnPositionX);
                float spawnPointY = Random.Range(minSpawnPositionY, maxSpawnPositionY);
                spawnPosition = new Vector3(spawnPointX, spawnPointY, 0);

                // Check if position is free (no enemies nearby)
                positionFound = true;
                foreach (var pos in occupiedPositions)
                {
                    if (Vector3.Distance(pos, spawnPosition) < enemyAreaRadius)
                    {
                        positionFound = false;
                        break;
                    }
                }

                attempts++;
            } while (!positionFound && attempts < 20); // Prevent infinite loops

            if (positionFound)
            {
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemies.Add(spawnedEnemy);
                occupiedPositions.Add(spawnPosition);
            }
            else
            {
                Debug.LogWarning("Could not find a valid spawn position after multiple attempts.");
            }
        }

        yield return StartCoroutine(WaitNextPattern(enemies));

        Debug.Log("TestPattern Ended");
    }

    private void Start() {
        SetDifficultyGraph();
    }

    [ContextMenu("SetDifficultyGraph")]
    public void SetDifficultyGraph(){
        minEnemySpawnCntGraph.keys = new Keyframe[0];
        maxEnemySpawnCntGraph.keys = new Keyframe[0];

        foreach(WaveDifficultyPoint point in min){
            minEnemySpawnCntGraph.AddKey(point.wave, point.value);
        }

        for (int i = 0; i < minEnemySpawnCntGraph.keys.Length; i++){
            AnimationUtility.SetKeyLeftTangentMode(minEnemySpawnCntGraph, i , AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(minEnemySpawnCntGraph, i , AnimationUtility.TangentMode.Constant);
        }

        foreach(WaveDifficultyPoint point in max){
            maxEnemySpawnCntGraph.AddKey(point.wave, point.value);
        }

        for (int i = 0; i < maxEnemySpawnCntGraph.keys.Length; i++){
            AnimationUtility.SetKeyLeftTangentMode(maxEnemySpawnCntGraph, i , AnimationUtility.TangentMode.Constant);
            AnimationUtility.SetKeyRightTangentMode(maxEnemySpawnCntGraph, i , AnimationUtility.TangentMode.Constant);
        }
    }


    protected override void CalculateActualWaveVariable(int wave)
    {
        if (wave == -1){
            return;
        }

        actualMinSpawnEnemyCnt = (int)minEnemySpawnCntGraph.Evaluate(wave);
        actualMaxSpawnEnemyCnt = (int)maxEnemySpawnCntGraph.Evaluate(wave);
    }
}

