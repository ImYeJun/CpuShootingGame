using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
        // Destroy(gameObject);
    
        DestroyTracker.DestroyWithLog(gameObject);
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
            // Debug.Log("Despawned by touching the DespwanArea");
            // Destroy(gameObject);
            DestroyTracker.DestroyWithLog(gameObject);
        }
    }

    public IEnumerator ExecuteHitEffect()
    {
        spriteRenderer.color = new Color(0.8f, 0.0f, 0.0f);

        float minHitEffectTime = 0.05f;
        
        SoundEffectManager.Instance.PlayWithRandomPitch(SoundEffectType.EnemyHit);

        yield return new WaitForSeconds(minHitEffectTime);

        isHit = false;

        spriteRenderer.color = originColor;
    }


}

public class DestroyTracker : MonoBehaviour
{
    public static void DestroyWithLog(GameObject obj,
        [CallerFilePath] string file = "",
        [CallerMemberName] string method = "",
        [CallerLineNumber] int line = 0)
    {
        Debug.Log($"Destroy() called on {obj.name} from {file}, method: {method}, line: {line}");
        Destroy(obj);
    }
}
