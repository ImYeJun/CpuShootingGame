using UnityEngine;

public class TestingEnemy : Mob
{
    [SerializeField] private float speed;

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
        SoundEffectManager.Instance.PlayWithRandomPitch(SoundEffectType.SeagullDead);
        base.OnDeath();
    }
}
