using UnityEngine;

public class PlayerBasicBullet : Bullet
{
    private bool isActivated = false;

    private void OnEnable() {
        InitiateBullet();
    }

    public override void InitiateBullet()
    {
        isActivated = true;
        rb.linearVelocity = transform.right.normalized * speed;
    }


    //* Object Pooling Object is implemented in this code
    //* This causes coupling
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(!isActivated){
            return;
        }

        if (other.tag == "Border"){
            Debug.Log("Bullet border Hit!");
            isActivated = false;
            PlayerBulletManager.Instance.ReturnBullet(gameObject);
        }

        if (gameObject.tag == "PlayerBullet" && other.tag == "Enemy"){
            Debug.Log("Player bullet hit enemy");
            isActivated = false;
            PlayerBulletManager.Instance.ReturnBullet(gameObject);
        }
    }
}
