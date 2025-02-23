using System.Collections;
using UnityEditor.Analytics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int killScore;
    [SerializeField] protected string enemyName;
    [SerializeField] protected string sorting;
    [SerializeField] protected GameObject bulletPrefab;
    private Color originColor;
    protected bool isHit = false;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    // This method is about implementing algorithm of enemy shooting bullets
    abstract public void ShootBullet();

    // This method is about implementing algorithm when enemy is dead
    virtual public void OnDeath(){
        CancelInvoke(nameof(ShootBullet));

        ScoreManager.Instance.AddScore(killScore);
        Destroy(gameObject);
    }

    public void DecreaseHealth(int damage = 1){
        health -= damage;

        if (health <= 0){
            OnDeath();
        }
    }

    virtual public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet"){
            int damage = collision.gameObject.GetComponent<Bullet>().Damage;

            DecreaseHealth(damage);

            isHit = true;
            StartCoroutine(ExecuteHitEffect());
        }

        if (collision.tag == "DeSpawnArea"){
            Debug.Log("Despawned by touching the DespwanArea");
            Destroy(gameObject);
        }
    }


    // TODO : When Enemy is hit by PlayerBullet, its color gets red and soon returns to its origin color
    public IEnumerator ExecuteHitEffect()
    {
        spriteRenderer.color = new Color(0.8f, 0.0f, 0.0f);

        float minHitEffectTime = 0.05f;
        
        SoundEffectManager.Instance.PlayWithRandomPitch(SoundEffectType.EnemyHit);

        yield return new WaitForSeconds(minHitEffectTime);

        isHit = false;  // 여기에서 false로 변경

        spriteRenderer.color = originColor;
    }


}
