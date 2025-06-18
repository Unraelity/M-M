using UnityEngine;

public class AnimationController : MonoBehaviour, IAnimator {

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;

    protected virtual void Awake() {
        
        if (animator == null) {

            animator = GetComponentInChildren<Animator>();

            if (animator == null) {
                Debug.LogError("No Animator Component Attached");
            }
        }
    }

    public void SetAnimatorTrigger(string boolString) {
        animator.SetTrigger(boolString);
    }

    public void SetAnimatorBool(string boolString, bool boolValue) {
        animator.SetBool(boolString, boolValue);
    }
}
