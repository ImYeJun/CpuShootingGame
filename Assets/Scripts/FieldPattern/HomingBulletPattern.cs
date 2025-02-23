using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBulletPattern : FieldPattern
{
    // TODO : Fix bug that Spawned Enemies are sometimes overlapped
    // TODO : Fix but that each HomingBulletPattern's spawnChecker is not shared
    public override IEnumerator ExecutePattern(int wave = 1)
    {
        Debug.Log("HomingBulletPattern Exectued");
    
        int spawnEnemyCnt = Random.Range(minSpawnEnemyCnt, maxSpawnEnemyCnt);

        float minSpawnPositionX = cameraTopLeftCoord.x + 0.4f;
        float maxSpawnPositionX = cameraTopRightCoord.x - 0.4f;
        float middleSpawnPositionX = (minSpawnPositionX + maxSpawnPositionX);
        List<float> xSpawnPosition = new List<float> {minSpawnPositionX, middleSpawnPositionX, maxSpawnPositionX};
        
        float minSpawnPositionY = cameraTopLeftCoord.y - 1.0f;
        float maxSpawnPositionY = cameraTopLeftCoord.y - 5.0f;
        float middleSpawnPositionY = (minSpawnPositionY + maxSpawnPositionY);
        List<float> ySpawnPosition = new List<float> {minSpawnPositionY, middleSpawnPositionY, maxSpawnPositionY};

        List<List<bool>> spawnChecker = new List<List<bool>>
        {
            new List<bool> { false, false, false },
            new List<bool> { false, false, false },
            new List<bool> { false, false, false }
        };

        for (int i = 0; i < spawnEnemyCnt; i++){
            bool positionFound = false;
            int xSpawnPositionIndex = -1;
            int ySpawnPositionIndex = -1;

            do{
                xSpawnPositionIndex = Random.Range(0, xSpawnPosition.Count);
                ySpawnPositionIndex = Random.Range(0, ySpawnPosition.Count);

                bool spawnPosition = spawnChecker[xSpawnPositionIndex][ySpawnPositionIndex];

                if (spawnPosition != false){
                    spawnChecker[xSpawnPositionIndex][ySpawnPositionIndex] = true;
                    positionFound = true;
                }
            }while(positionFound);

            Vector3 spawnPositionCoordinate = new Vector3(
                xSpawnPosition[xSpawnPositionIndex],
                ySpawnPosition[ySpawnPositionIndex],
                0
                );
            
            GameObject spawnEnemy = Instantiate(enemyPrefab, spawnPositionCoordinate, Quaternion.identity);
        }

        yield return new WaitForSeconds(coolTimeAfterExecuted);

        Debug.Log("HomingBulletPattern Ended");
    }
}
