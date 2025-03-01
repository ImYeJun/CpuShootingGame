using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private Vector3 moveDirection;
    Animator animator;
    
    [SerializeField] private GameObject moveObject; // 이동할 오브젝트
    [SerializeField] private Camera mainCamera;
    private float tailLength = 0.35f;

    private void Awake() {
        animator = moveObject.GetComponent<Animator>();
        GetCameraEdgeCoordinate();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousPosition = ScreenToWorld(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPosition = ScreenToWorld(eventData.position);
        moveDirection = currentPosition - previousPosition;

        PlayerController playerController = moveObject.GetComponent<PlayerController>();

        if (Math.Abs(moveDirection.x) >= 0.07f){
                animator.SetBool("IsTitled", true);
                if (moveDirection.x < 0){
                    animator.SetFloat("Direction", 0);
                }
                else{
                    animator.SetFloat("Direction", 1);
                }
        }
        else{
            animator.SetBool("IsTitled", false);
        }

        playerController.UpdatePlayerCoord();

        if (playerController.TopLeft.x + moveDirection.x <= cameraBottomLeftCoord.x || 
            playerController.TopRight.x + moveDirection.x >= cameraBottomRightCoord.x ||
            playerController.BottomRight.x + moveDirection.x <= cameraBottomLeftCoord.x ||
            playerController.BottomLeft.x + moveDirection.x >= cameraBottomRightCoord.x){
            moveDirection.x = 0.0f;
        }

        if (playerController.TopLeft.y + moveDirection.y >= cameraTopLeftCoord.y|| 
            playerController.TopRight.y + moveDirection.y >= cameraTopRightCoord.y ||
            playerController.BottomRight.y + moveDirection.y <= cameraBottomRightCoord.y - tailLength||
            playerController.BottomLeft.y + moveDirection.y <= cameraBottomRightCoord.y - tailLength){
            moveDirection.y = 0.0f;
        }

        moveObject.transform.position += moveDirection;

        previousPosition = currentPosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        animator.SetBool("IsTitled", false);
    }

    private Vector3 ScreenToWorld(Vector3 screenPosition)
    {
        return mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane + 5f));
    }

    protected Vector3 cameraTopRightCoord;
    protected Vector3 cameraTopLeftCoord;
    protected Vector3 cameraBottomRightCoord;
    protected Vector3 cameraBottomLeftCoord;

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
