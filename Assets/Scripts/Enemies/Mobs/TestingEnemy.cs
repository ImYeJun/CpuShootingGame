using UnityEngine;

public class TestingEnemy : Mob
{
    [SerializeField] private float speed;
    public float Speed{
        get{
            return speed;
        }
        set{
            if (value < 0){
                speed = 0;
                return;
            }

            speed = value;
        }
    }

    override protected void Awake()
    {
        base.Awake();
    }

    public void StartToMove(){
        rb.linearVelocity = new Vector2(0, -speed);
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
