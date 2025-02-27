using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class FieldPattern : MonoBehaviour
{
    [SerializeField] protected GameObject enemyPrefab;
    [SerializeField] protected float coolTimeAfterExecuted;
    [SerializeField] protected float coolTimeAfterClear;
    public float CoolTimeAfterExecuted => coolTimeAfterExecuted;

    protected List<GameObject> enemies = new List<GameObject>();

    [SerializeField] protected int minSpawnEnemyCnt;
    public int MinSpawnEnemyCnt => minSpawnEnemyCnt;

    [SerializeField] protected int maxSpawnEnemyCnt;
    public int MaxSpawnEnemyCnt => maxSpawnEnemyCnt;

    public abstract IEnumerator ExecutePattern(int wave = 1);

    private Camera mainCamera;
    protected Vector3 cameraTopRightCoord;
    protected Vector3 cameraTopLeftCoord;
    protected Vector3 cameraBottomRightCoord;
    protected Vector3 cameraBottomLeftCoord;

    virtual protected void Awake() {
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
}
