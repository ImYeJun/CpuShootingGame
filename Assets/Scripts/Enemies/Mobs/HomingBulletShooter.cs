using System.Collections;
using UnityEngine;

public class HomingBulletShooter : Mob
{
    [SerializeField] private float coolTimeAfterSpawn;
    [SerializeField] private float shootIntervalTime;
    [SerializeField] private float rotateTime;
    [SerializeField] private AnimationCurve convolution;
    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(ShootBullet), coolTimeAfterSpawn, shootIntervalTime);
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
        // CancelInvoke(nameof(ShootBullet));
        base.OnDeath();
    }
}
