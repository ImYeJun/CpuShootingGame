using Unity.VisualScripting;
using UnityEngine;

public class Eagle : Mob
{
    private Vector3 facingDirection;
    private Vector3 playerFacingDirection;
    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerFacingDirection = (player.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(playerFacingDirection.y, playerFacingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        facingDirection = gameObject.transform.right.normalized;
        
        InvokeRepeating(nameof(ShootBullet),0.2f, 0.7f);
    }

    //! Actually don't know why it works perfectly. I should study about this math shit.
    public override void ShootBullet()
    {
        float basicAngle = Mathf.Atan2(-facingDirection.y, -facingDirection.x) * Mathf.Rad2Deg;

        for (int i = -1; i <= 1; i++){
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullet.transform.rotation = Quaternion.Euler(0, 0, basicAngle) * Quaternion.Euler(0,0, 15 * i);
        }
    }
}
