using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float correctionGap;
    private float spriteHeight;
    void Awake()
    {
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        Vector3 moveVelocity = new Vector3(0,1,0) * speed * Time.deltaTime;
        transform.position = transform.position + moveVelocity;

        if (transform.position.y > spriteHeight){
            transform.position = new Vector3(0, -spriteHeight + correctionGap, 0);
        }
    }
}
