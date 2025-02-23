using UnityEngine;

public class HomingBullet : Bullet
{
    public override void InitiateBullet()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 homingDirection = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = homingDirection * speed;

        float angle = Mathf.Atan2(homingDirection.y, homingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
