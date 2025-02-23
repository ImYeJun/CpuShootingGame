using UnityEngine;

public class HomingBulletShooter : Mob
{
    [SerializeField] private float coolTimeAfterSpawn;
    [SerializeField] private float shootIntervalTime;
    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(ShootBullet), coolTimeAfterSpawn, shootIntervalTime);
    }

    public override void ShootBullet()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 facingDirection = (player.transform.position - transform.position).normalized;

        float spawnPositionOffset = 0.92f;
        Vector3 spawnPosition = transform.position + facingDirection * spawnPositionOffset;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);


        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void OnDeath()
    {
        Debug.Log("Homing Bullet Shooter is Dead");
        // CancelInvoke(nameof(ShootBullet));
        base.OnDeath();
    }
}
