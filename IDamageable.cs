using UnityEngine;

public interface IDamageable {
    public GameObject HitBox {
        get;
    }
    
    public void TakeDamage(float damage);
}
