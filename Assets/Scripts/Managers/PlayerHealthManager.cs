using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int initialHaelth;
    [SerializeField] private float invincibilityTime;
    private bool isInvincible = false;
    public int InitialHaelth{
        get{
            return initialHaelth;
        }
    }
    [SerializeField] private int _health;
    [SerializeField] private GameObject heartIconContainer;
    [SerializeField] private GameObject heartIconPrefab;

    private void Awake() {
        IncreaseHealth(initialHaelth);
    }

    public void DecreaseHealth(int damage = 1){
        if (isInvincible){
            return;
        }
        
        _health -= damage;

        StartCoroutine(ExecuteDeadEffect());

        if (_health <= 0){
            Debug.Log("Player is Dead. Game Over!");
            GameSceneManager.Instance.LoadScene(SceneType.GameOverScene);
            return;
        }

        for (int i = 0; i < damage; i++){
            GameObject heartIcon = heartIconContainer.transform.GetChild(0).gameObject;

            if (heartIcon == null){
                break;
            }

            Destroy(heartIcon);
        }
    }

    public void IncreaseHealth(int point = 1){
        _health += point;

        for (int i = 0; i < point; i++){
            GameObject heartIcon = Instantiate(heartIconPrefab);

            heartIcon.transform.SetParent(heartIconContainer.transform);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet"){
            DecreaseHealth();
        }
    }


    // TODO : Fix bug that sprites are not fully red
    public IEnumerator ExecuteDeadEffect()
    {
        isInvincible = true;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color initialColor = spriteRenderer.color;
        bool isTransparernt = false;


        float blinkInterval = 0.4f; // Adjust this value for faster or slower blinking
        float elapsedTime = 0f;

        SoundEffectManager.Instance.Play(SoundEffectType.PlayerHit);

        while (elapsedTime < invincibilityTime)
        {
            // Toggle between visible and semi-transparent
            Color color = new Color();

            color.a = isTransparernt ? 1.0f : 0.8f;
            isTransparernt = !isTransparernt;

            color.r = 255.0f / 255.0f;
            color.g = 129.0f / 255.0f;
            color.b = 129.0f / 255.0f;

            spriteRenderer.color = color;

            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        // Restore original state
        isInvincible = false;
        spriteRenderer.color = initialColor;
    }
}
