using UnityEngine;

abstract public class Bullet : MonoBehaviour
{
    [SerializeField] protected int damage;
    public int Damage{
        get{
            return damage;
        }
    }
    [SerializeField] protected int speed;
    protected Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitiateBullet();
    }

    abstract public void InitiateBullet();

    virtual protected void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Border"){
            Debug.Log("Bullet border Hit!");
            gameObject.SetActive(false);
        }

        if (gameObject.tag == "PlayerBullet" && other.tag == "Enemy"){
            Debug.Log("Player bullet hit enemy");
            gameObject.SetActive(false);
        }

        if (gameObject.tag == "EnemyBullet" && other.tag == "Player"){
            Debug.Log("Enemy bullet hit player");
            gameObject.SetActive(false);
        }
    }
}
