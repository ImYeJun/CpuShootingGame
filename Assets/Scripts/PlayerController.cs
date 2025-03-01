using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;

    [SerializeField] private float topGrazeDetectLength;
    [SerializeField] private float rightGrazeDetectLength;
    [SerializeField] private float grazeIntervalTime;
    [SerializeField] private Color gizmoColor;

    private SpriteRenderer spriteRenderer;
    private Vector2 topLeft;
    public Vector2 TopLeft => topLeft;
    private Vector2 topRight;
    public Vector2 TopRight => topRight;
    private Vector2 bottomLeft;
    public Vector2 BottomLeft => bottomLeft;
    private Vector2 bottomRight;
    public Vector2 BottomRight => bottomRight;

    public GameObject PlayerBasicBullet; //* this code is only for testing
    public float shootInterval = 0.1f; // Shooting every 1 second
    private Animator animator;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void UpdatePlayerCoord() {
        Vector2 min = spriteRenderer.bounds.min; // Bottom-left
        Vector2 max = spriteRenderer.bounds.max; // Top-right

        topLeft = new Vector2(min.x, max.y);
        topRight = max;
        bottomLeft = min;
        bottomRight = new Vector2(max.x, min.y);
    }

    //* this code is only for testing
    private void Start() {
        InvokeRepeating(nameof(Shoot), 0f, shootInterval); // Call Shoot() repeatedly every shootInterval seconds
        InvokeRepeating(nameof(CheckGrazing), 0f, grazeIntervalTime);
    }

    //* this code is only for testing
    public void Shoot(){
        Debug.Log("Shooting!");

        GameObject bullet = PlayerBulletManager.Instance.GetBullet();

        bullet.transform.position = transform.position;
        bullet.SetActive(true);
    }

    public void CheckGrazing(){
        Vector2 grazeAreaTopLeft = new Vector2(topLeft.x - rightGrazeDetectLength, topLeft.y + topGrazeDetectLength);
        Vector2 grazeAreaBottomRight = new Vector2(bottomRight.x + rightGrazeDetectLength, bottomRight.y - topGrazeDetectLength);

        Collider2D[] detectedColliders = Physics2D.OverlapAreaAll(grazeAreaTopLeft, grazeAreaBottomRight);

        foreach(Collider2D collider in detectedColliders){
            if (collider.tag == "EnemyBullet" || collider.tag == "Enemy"){
                ScoreManager.Instance.AddScore(10);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!TryGetComponent(out SpriteRenderer spriteRenderer)) return;

        Vector2 min = spriteRenderer.bounds.min;
        Vector2 max = spriteRenderer.bounds.max;

        Vector2 topLeft = new Vector2(min.x - rightGrazeDetectLength, max.y + topGrazeDetectLength);
        Vector2 bottomRight = new Vector2(max.x + rightGrazeDetectLength, min.y - topGrazeDetectLength);

        // Calculate center and size
        Vector2 center = (topLeft + bottomRight) / 2;
        Vector2 size = new Vector2(Mathf.Abs(bottomRight.x - topLeft.x), Mathf.Abs(bottomRight.y - topLeft.y));

        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(center, size);
    }

}
