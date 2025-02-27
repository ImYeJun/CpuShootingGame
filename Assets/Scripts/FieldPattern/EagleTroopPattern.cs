using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleTroopPattern : FieldPattern
{
    [SerializeField] private float topSpawnGap;
    [SerializeField] private float bottomSpawnGap;
    [SerializeField] private float enemyAreaRadius;
    private int levelUpWaveUnit = 5;

    public override IEnumerator ExecutePattern(int wave = -1)
    {
        List<GameObject> enemies = new List<GameObject>();

        Debug.Log("EagleTroopPattern Exectued");
    
        int spawnEnemyCnt = Random.Range(minSpawnEnemyCnt, maxSpawnEnemyCnt);
        List<Vector3> spawnPoints = new List<Vector3>();
        int maxSpawnTrial = 20;

        for (int i = 0; i < spawnEnemyCnt; i++){
            bool isPositionFound = true;
            Vector3 spawnPos = new Vector3();
            int spawnTrial = 0;

            do{
                isPositionFound = true;

                float spawnX = Random.Range(cameraBottomLeftCoord.x + 0.5f, cameraBottomRightCoord.x - 0.5f);
                float spawnY = Random.Range(cameraTopLeftCoord.y - topSpawnGap, cameraBottomLeftCoord.y + bottomSpawnGap);
                
                spawnPos = new Vector3(spawnX, spawnY , 0);

                if (spawnTrial >= maxSpawnTrial){
                    Debug.Log("Eagle Spawn Max Trial Exceeded");
                    break;
                }

                foreach(Vector3 pos in spawnPoints){
                    float distance = Vector3.Distance(spawnPos, pos);

                    if (distance < enemyAreaRadius){
                        isPositionFound = false;
                        break;
                    }
                }

                spawnTrial++;
            }while(!isPositionFound);

            spawnPoints.Add(spawnPos);
        }

        for (int i = 0; i <spawnEnemyCnt; i++){
            GameObject spawnEnemy = Instantiate(enemyPrefab, spawnPoints[i], Quaternion.identity);
            enemies.Add(spawnEnemy);
        }

        yield return StartCoroutine(WaitNextPattern(enemies));

        Debug.Log("EagleTroopPattern Ended");
    }
    protected override void CalculateActualWaveVariable(int wave)
    {
        throw new System.NotImplementedException();
    }
}
