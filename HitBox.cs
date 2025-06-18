using UnityEngine;

public class HitBox : MonoBehaviour {

    [SerializeField] private BaseController controller;
    private IDamageable damage;

    public IDamageable Damage {
        get { return damage; }
    }

    public BaseController Controller {
        get { return controller; }
    }

    private void Awake() {

        if (controller == null) {
            controller = GetComponentInParent<BaseController>();

            if (controller == null) {
                Debug.LogError("Hit Box cannot find Controller script");
            }
        }

        damage = GetComponentInParent<IDamageable>();

        if (damage == null) {
            Debug.LogError("Hit Box cannot find IDamageable reference");
        }
    }
}
