using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

abstract public class FieldPattern : MonoBehaviour
{
    [Header("Pattern Common Field")]
    [SerializeField] protected GameObject enemyPrefab;
    
    [Header("Difficulty Field")]
    [Space(5)]
    [SerializeField] protected float coolTimeAfterExecuted;
    public float CoolTimeAfterExecuted => coolTimeAfterExecuted;
    [SerializeField]protected float actualCoolTimeAfterExectued;

    [Space(5)]
    [SerializeField] protected float coolTimeAfterClear;
    public float CoolTimeAfterClear => coolTimeAfterClear;
    [SerializeField]protected float actualCoolTimeAfterClear;

    // protected List<List<GameObject>> allWaveEnemies = new List<List<GameObject>>();

    [Space(5)]
    [SerializeField] protected int minSpawnEnemyCnt;
    public int MinSpawnEnemyCnt => minSpawnEnemyCnt;
    [SerializeField]protected int actualMinSpawnEnemyCnt;
    
    [Space(5)]
    [SerializeField] protected int maxSpawnEnemyCnt;
    public int MaxSpawnEnemyCnt => maxSpawnEnemyCnt;
    [SerializeField]protected int actualMaxSpawnEnemyCnt;

    [Space(7)]
    [Header("Difficulty Graph")]
    [SerializeField] protected AnimationCurve minEnemySpawnCntGraph;
    [SerializeField] protected AnimationCurve maxEnemySpawnCntGraph;

    //* -1 means that the executed wave number is under earlyPatternIndex.
    public abstract IEnumerator ExecutePattern(int wave = -1);
    protected abstract void CalculateActualWaveVariable(int wave);

    private Camera mainCamera;
    protected Vector3 cameraTopRightCoord;
    protected Vector3 cameraTopLeftCoord;
    protected Vector3 cameraBottomRightCoord;
    protected Vector3 cameraBottomLeftCoord;

    virtual protected void Awake() {
        actualCoolTimeAfterClear = coolTimeAfterClear;
        actualCoolTimeAfterExectued = coolTimeAfterExecuted;
        actualMaxSpawnEnemyCnt = maxSpawnEnemyCnt;
        actualMinSpawnEnemyCnt = minSpawnEnemyCnt;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        GetCameraEdgeCoordinate();
    }


    private void GetCameraEdgeCoordinate(){
            // Get the orthographic size and aspect ratio of the camera
            float size = mainCamera.orthographicSize;
            float aspect = mainCamera.aspect;

            // Calculate the half-width and half-height based on the orthographic size and aspect ratio
            float halfHeight = size;
            float halfWidth = size * aspect;

            // Calculate the positions of the four edges in 2D (X and Y only)
            cameraTopRightCoord = mainCamera.transform.position + new Vector3(halfWidth, halfHeight, 0);
            cameraTopLeftCoord = mainCamera.transform.position + new Vector3(-halfWidth, halfHeight, 0);
            cameraBottomRightCoord = mainCamera.transform.position + new Vector3(halfWidth, -halfHeight, 0);
            cameraBottomLeftCoord = mainCamera.transform.position + new Vector3(-halfWidth, -halfHeight, 0);
    }

    virtual public IEnumerator WaitNextPattern(List<GameObject> enemies){
        float startTime = Time.time;
        float elapsedTime = 0.0f;

        while (elapsedTime <= coolTimeAfterExecuted){
            elapsedTime = Time.time - startTime;

            int activatedEnemyCnt = enemies.Count(e => e != null);

            if (activatedEnemyCnt <= 0){
                Debug.Log("All Enemies are Dead!");
                yield return new WaitForSeconds(coolTimeAfterClear);

                yield break;
            }

            yield return null;
        }
    }
}

//! Add this logic to Blueprint
[Serializable]
public struct WaveDifficultyPoint{
    public int wave;
    public int value;
}