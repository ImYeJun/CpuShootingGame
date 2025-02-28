using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomingBulletPattern : FieldPattern
{    
    [Space(10)]
    [Header("HomingBulletPattern Field")]
    [SerializeField] bool isDrawingActivate;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPoints;  // Store the path points
    private Vector3 drawPoint;
    [SerializeField] float drawTime;
    [SerializeField] private float bottomSpawnCoordCorrectionValue;
    [SerializeField] private float topSpawnCoordCorrectionValue;
    [SerializeField] private float sideSpawnCoordCorrectionValue;
    [SerializeField] private List<GameObject> spawnLinePoint;
    [SerializeField] private int corretionToPlayerPoint;
    [SerializeField] private float enemyAreaRadius;

    private List<Vector3> spawnLinePointCoords = new List<Vector3>() 
    {
        new Vector3(),
        new Vector3(),
        new Vector3(),
        new Vector3()
    };

    protected override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;
    private Vector3 p4;

    private void Start() {
        // Set line properties (optional)
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;  // Start with no points in the line

        pathPoints = new List<Vector3>();  // Initialize pathPoints list

    }

    public override IEnumerator ExecutePattern(int wave = -1)
    {
        List<GameObject> enemies = new List<GameObject>();
        CalculateActualWaveVariable(wave);

        Debug.Log("HomingBulletPattern Exectued");
    
        int spawnEnemyCnt = Random.Range(actualMinSpawnEnemyCnt, actualMaxSpawnEnemyCnt);
        List<Vector3> spawnPoints = GetSpawnPoints(spawnEnemyCnt);

        for (int i = 0; i < spawnEnemyCnt; i++){
            GameObject spawnEnemy = Instantiate(enemyPrefab, spawnPoints[i], Quaternion.identity);
            enemies.Add(spawnEnemy);
        }
        
        yield return WaitNextPattern(enemies);

        Debug.Log("HomingBulletPattern Ended");
    }

    // * This code is only for testing
    // private void Update() {
    //     if (isDrawingActivate){
    //         foreach (GameObject point in spawnLinePoint){
    //             point.SetActive(true);
    //         }
    //     }
    //     else{
    //         pathPoints.Add(drawPoint);
    //         lineRenderer.positionCount = pathPoints.Count;
    //         lineRenderer.SetPositions(pathPoints.ToArray());

    //         foreach (GameObject point in spawnLinePoint){
    //             if (point == null){
    //                 return;
    //             }

    //             point.SetActive(false);
    //         }

    //         ExecuteDrawSpawnLine();
    //     }
    // }

    public List<Vector3> GetSpawnPoints(int cnt){
        List<Vector3> spawnPoints = new List<Vector3>();
        Vector3 spawnPoint = new Vector3();
        
        for (int i = 0; i < spawnLinePointCoords.Count; i++){
            float x = Random.Range(cameraTopLeftCoord.x,cameraTopRightCoord.x);
            float y = Random.Range(cameraBottomLeftCoord.y,cameraTopLeftCoord.y);
            
            Vector3 pointCoord = new Vector3(x, y, 0);
            spawnLinePointCoords[i] = pointCoord;
        }

        Vector3 firstCoordCorrection = new Vector3(
            spawnLinePointCoords[0].x, 
            Random.Range(cameraTopLeftCoord.y - 3.0f,cameraTopLeftCoord.y - 1.0f), 
            spawnLinePointCoords[0].z);

        spawnLinePointCoords[0] = firstCoordCorrection;

        Vector3 secondCoordCorrection = new Vector3(
            Random.Range(cameraBottomLeftCoord.x,(cameraBottomLeftCoord.x+cameraBottomRightCoord.x)/2), 
            spawnLinePointCoords[1].y, 
            spawnLinePointCoords[1].z
        );
        spawnLinePointCoords[1] = secondCoordCorrection;

        Vector3 thirdCoordCorrection = new Vector3(
            Random.Range((cameraBottomLeftCoord.x+cameraBottomRightCoord.x)/2, cameraBottomRightCoord.x), 
            spawnLinePointCoords[2].y, 
            spawnLinePointCoords[2].z
        );
        spawnLinePointCoords[2] = thirdCoordCorrection;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        spawnLinePointCoords[corretionToPlayerPoint] = new Vector3(
            playerPos.x, 
            Mathf.Clamp(playerPos.y, cameraBottomLeftCoord.y + 2.0f,cameraTopLeftCoord.y),
            playerPos.z
            );


        int maxSpawnTrial = 20;
        for (int i = 0; i < cnt; i++){
            bool isPositionFound = true;
            int spawnTrial = 0;

            do{
                isPositionFound = true;

                float t = Random.Range(0.0f, 1.0f);

                p0 = Vector3.Lerp(spawnLinePointCoords[0], spawnLinePointCoords[1], t);
                p1 = Vector3.Lerp(spawnLinePointCoords[1], spawnLinePointCoords[2], t);
                p2 = Vector3.Lerp(spawnLinePointCoords[2], spawnLinePointCoords[3], t);

                p3 = Vector3.Lerp(p0, p1, t);
                p4 = Vector3.Lerp(p1, p2, t);

                spawnPoint = Vector3.Lerp(p3, p4, t);
                float xCorrectionValue = GetxCorrectionValue(t);
                float yCorrectionValue = GetyCorrectionValue(t);

                spawnPoint = new Vector3(
                    Mathf.Clamp(spawnPoint.x + xCorrectionValue, cameraBottomLeftCoord.x + sideSpawnCoordCorrectionValue, cameraBottomRightCoord.x - sideSpawnCoordCorrectionValue),
                    Mathf.Clamp(spawnPoint.y + yCorrectionValue, cameraBottomLeftCoord.y + bottomSpawnCoordCorrectionValue, cameraTopLeftCoord.y - topSpawnCoordCorrectionValue),
                    spawnPoint.z
                );
                
                if(spawnTrial >= maxSpawnTrial){
                    Debug.Log("HomingBulletSpawner Spawn Max Trial Exceeded");
                    break;
                }

                foreach(Vector3 pos in spawnPoints){
                    float distance = Vector3.Distance(spawnPoint, pos);

                    if (distance < enemyAreaRadius){
                        isPositionFound = false;
                        break;
                    }
                }

                spawnTrial++;
            }while(!isPositionFound);

            spawnPoints.Add(spawnPoint);
        }

        // * This code is only for testing
        // ExecuteDrawSpawnLine(); 

        return spawnPoints;
    }

    private IEnumerator DrawSpawnLine(){
        pathPoints = new List<Vector3>();

        for (int i = 0; i < spawnLinePoint.Count; i++){
            spawnLinePoint[i].transform.position = spawnLinePointCoords[i];
        }   

        float startTime = Time.time;
        float currentPer = 0.0f;

        while (currentPer < 1.0f){
            float elapsedTime = Time.time - startTime;
            currentPer = Mathf.Clamp01(elapsedTime / drawTime);

            p0 = Vector3.Lerp(spawnLinePointCoords[0], spawnLinePointCoords[1], currentPer);
            p1 = Vector3.Lerp(spawnLinePointCoords[1], spawnLinePointCoords[2], currentPer);
            p2 = Vector3.Lerp(spawnLinePointCoords[2], spawnLinePointCoords[3], currentPer);

            p3 = Vector3.Lerp(p0, p1, currentPer);
            p4 = Vector3.Lerp(p1, p2, currentPer);

            drawPoint = Vector3.Lerp(p3, p4, currentPer);

            transform.position = drawPoint;

            // Add current point to the path and update the line renderer
            pathPoints.Add(drawPoint);
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());

            yield return null;
        }
    }
    
    private void ExecuteDrawSpawnLine(){
        StopCoroutine(nameof(DrawSpawnLine));

        StartCoroutine(nameof(DrawSpawnLine));
    }

    private float GetTCorrectionValue(float t)
    {
        float[] criticalPoints = { 0.0f, 0.25f, 0.5f, 0.75f, 1.0f };

        if (criticalPoints.Contains(t))
            return 0.0f;

        return Mathf.Sin(1000000.0f / (t * (0.25f - t) * (0.5f - t) * (0.75f - t) * (1.0f - t)));
    }

    private float GetxCorrectionValue(float t){
        float[] ciriticalPoints = { -1.0f, -0.75f, -0.5f, 0.0f, 0.5f, 0.75f, 1.0f};

        if (ciriticalPoints.Contains(t)){
            return 0.0f;
        }

        return 1.5f * Mathf.Sin(1000000.0f / ((1.0f + t) * (0.75f + t) * (0.5f + t) * t * (0.5f - t) * (0.75f - t) * (1.0f - t)));
    }

    private float GetyCorrectionValue(float t){
        float[] ciriticalPoints = { -1.0f, -0.75f, -0.5f, 0.0f, 0.5f, 0.75f, 1.0f};

        if (ciriticalPoints.Contains(t)){
            return 0.0f;
        }

        return 2f * Mathf.Cos(1000000.0f / ((1.0f + t) * (0.75f + t) * (0.5f + t) * t *(0.5f - t) * (0.75f - t) * (1.0f - t)));
    }
}
