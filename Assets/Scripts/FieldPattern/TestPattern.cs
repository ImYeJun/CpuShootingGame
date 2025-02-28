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

}

