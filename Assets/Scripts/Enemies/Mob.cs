using System.Buffers.Text;
using UnityEngine;

public abstract class Mob : Enemy
{    
    override protected void Awake() {
        base.Awake();
        sorting = "Mob";
    }
    
    public abstract override void ShootBullet();
}
