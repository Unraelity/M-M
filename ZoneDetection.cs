using UnityEngine;

public class ZoneDetection : MonoBehaviour {

    private BaseController target = null;

    public BaseController Target {
        get { return target; }
    }
    public bool HasTarget { get { return target != null;}}

    private void OnTriggerEnter2D(Collider2D other) {

        HitBox hitBox = other.GetComponent<HitBox>();

        if (hitBox != null) {
            target = hitBox.Controller;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (target != null) {

            HitBox hitBox = other.GetComponent<HitBox>();

            if ((hitBox != null) && (target == hitBox.Controller)) {
                target = null;
            }
        }
    }
    
}
