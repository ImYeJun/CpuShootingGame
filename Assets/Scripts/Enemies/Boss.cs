using UnityEngine;

public abstract class Boss : Enemy
{    
    private void Awake() {
        sorting = "Boss";
    }
    
    public abstract override void ShootBullet();
}
