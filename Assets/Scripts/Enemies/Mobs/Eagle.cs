using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Eagle : Mob
{
    private Vector3 facingDirection;
    private Vector3 playerFacingDirection;
    [SerializeField] private float blinkIntervalTime;

    protected override void Awake()
    {
        base.Awake();
        
        StartCoroutine(nameof(ExecuteBlinkingEffect));
    }

    public override void OnDeath()
    {
        Debug.Log("Eagle is Dead");
        SoundEffectManager.Instance.PlayWithRandomPitch(SoundEffectType.DefaultEnemyDead);
        base.OnDeath();
    }

    public override void ShootBullet()
    {
        float basicAngle = Mathf.Atan2(-facingDirection.y, -facingDirection.x) * Mathf.Rad2Deg;

        for (int i = -1; i <= 1; i++){
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.transform.rotation = Quaternion.Euler(0, 0, basicAngle) * Quaternion.Euler(0,0, 30 * i);
        }
    }

    private IEnumerator ExecuteBlinkingEffect(){
        Color originColor = spriteRenderer.color;
        Color lowAlphaColor = originColor;
        Color highAlphaColor = originColor;

        rb.simulated = false;

        Quaternion rotateAngle = Quaternion.Euler(0,0, 180);
        transform.rotation = rotateAngle;

        lowAlphaColor.a = 0.5f;
        highAlphaColor.a = 0.8f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerFacingDirection = (player.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(playerFacingDirection.y, playerFacingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        facingDirection = gameObject.transform.right.normalized;
        
        for (int i = 0; i < 3; i++){

            spriteRenderer.color = lowAlphaColor;

            yield return new WaitForSeconds(blinkIntervalTime);

            spriteRenderer.color = highAlphaColor;
            
            yield return new WaitForSeconds(blinkIntervalTime);
        }

        spriteRenderer.color = originColor;
        rb.simulated = true;

        InvokeRepeating(nameof(ShootBullet),0.2f, 1.0f);
    }
}
