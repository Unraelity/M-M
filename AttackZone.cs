using UnityEngine;

public class AttackZone : MonoBehaviour {

    private float damage = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        
        HitBox hitBox = other.GetComponent<HitBox>();

        if (hitBox != null) {
            hitBox.Damage.TakeDamage(damage);
        }
    }

    public void SetDamage(float damage) {
        this.damage = damage;
    }
}
