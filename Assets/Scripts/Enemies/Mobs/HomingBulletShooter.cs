using System.Collections;
using UnityEngine;

public class HomingBulletShooter : Mob
{
    [SerializeField] private float coolTimeAfterSpawn;
    [SerializeField] private float shootIntervalTime;
    [SerializeField] private float rotateTime;
    [SerializeField] private AnimationCurve convolution;
    [SerializeField] private float blinkIntervalTime;
    protected override void Awake()
    {
        base.Awake();

        // InvokeRepeating(nameof(ShootBullet), coolTimeAfterSpawn, shootIntervalTime);
    }
    
    public void StartToMove(float shootIntervalRandomRange){
        float shootIntervalRandomValue = Random.Range(-shootIntervalRandomRange, shootIntervalRandomRange);

        rotateTime += shootIntervalRandomRange;
        shootIntervalTime += shootIntervalRandomRange;

        StartCoroutine(nameof(ExecuteBlinkingEffect));
    }

    public override void ShootBullet()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 facingDirection = (player.transform.position - transform.position).normalized;

        // float spawnPositionOffset = 0.92f;
        // Vector3 spawnPosition = transform.position + facingDirection * spawnPositionOffset;

        // GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;

        // ! If rotateTime is longer than shootInterval, it would have a issue  
        StartCoroutine(ShootBullet_(new Vector3(0,0, angle)));
    }

    private IEnumerator ShootBullet_(Vector3 target){
        float startTime = Time.time;
        float currentPer = 0.0f;
        Quaternion startAngle = transform.rotation;
        Quaternion endAngle = Quaternion.Euler(target);

        while (currentPer < 1.0f){
            float elapsedTime = Time.time - startTime;
            currentPer = Mathf.Clamp01(elapsedTime / rotateTime);
            
            Quaternion moveAngle = Quaternion.Slerp(startAngle, endAngle, convolution.Evaluate(currentPer));

            transform.rotation = moveAngle;

            yield return null;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 facingDirection = (player.transform.position - transform.position).normalized;

        float spawnPositionOffset = 0.92f;
        Vector3 spawnPosition = transform.position + facingDirection * spawnPositionOffset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }

    public override void OnDeath()
    {
        Debug.Log("Homing Bullet Shooter is Dead");
        SoundEffectManager.Instance.PlayWithRandomPitch(SoundEffectType.OtterDead);
        base.OnDeath();
    }

    private IEnumerator ExecuteBlinkingEffect(){
        Color originColor = spriteRenderer.color;
        Color lowAlphaColor = originColor;
        Color highAlphaColor = originColor;

        rb.simulated = false;

        Quaternion rotateAngle = Quaternion.Euler(0,0, -90);
        transform.rotation = rotateAngle;

        lowAlphaColor.a = 0.5f;
        highAlphaColor.a = 0.8f;

        for (int i = 0; i < 3; i++){
            spriteRenderer.color = lowAlphaColor;

            yield return new WaitForSeconds(blinkIntervalTime);

            spriteRenderer.color = highAlphaColor;
            
            yield return new WaitForSeconds(blinkIntervalTime);
        }

        spriteRenderer.color = originColor;
        rb.simulated = true;

        InvokeRepeating(nameof(ShootBullet), coolTimeAfterSpawn, shootIntervalTime);
    }
}
