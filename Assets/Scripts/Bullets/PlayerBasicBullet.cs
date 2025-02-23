using UnityEngine;

public class PlayerBasicBullet : Bullet
{
    public override void InitiateBullet()
    {
        rb.linearVelocity = transform.right.normalized * speed;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
