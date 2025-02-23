using UnityEngine;

public class TestingEnemy : Mob
{
    [SerializeField] private int speed;

    override protected void Awake()
    {
        base.Awake();
        rb.linearVelocity = new Vector2(0, -speed);
        // InvokeRepeating(nameof(ShootBullet), 1f, 1f); 
    }
    public override void ShootBullet()
    {
    }

    public override void OnDeath()
    {
        Debug.Log("TestingEnemy is Dead");
        // CancelInvoke(nameof(ShootBullet)); // Stop shooting when the enemy dies
        base.OnDeath();
    }
}
