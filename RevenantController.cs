using System.Collections;
using UnityEngine;

public class RevenantController : BaseController, IDamageable {

    protected enum State {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    [Header("Component Settings")]
    private OrientationController orientation;
    private MovementController movement;
    private AnimationController anim;
    [Header("Damage Settings")]
    [SerializeField] private GameObject hitBox;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackAnimTime = 0.750f;
    [SerializeField] private float attackCooldownTime = 3f;    // cooldown time for attack
    [SerializeField] private ZoneDetection detectionZone;
    [SerializeField] private ZoneDetection attackDetectionZone;
    [SerializeField] private AttackZone attackZone;
    private float health;
    private bool canAttack = true;                             // check if the attack is off cooldown
    private State currState = State.Idle;                      // current state
    private Vector3 targetPos = Vector3.zero;                  // target position for moving state
    // animation parameters
    private string movingAnimParam = "Moving";
    private string attackingAnimParam = "Attacking";
    private string deathAnimParam = "Dead";

    public GameObject HitBox => hitBox;

    private void Awake() {

        if (orientation == null) {

            orientation = GetComponent<OrientationController>();

            if (orientation == null) {
                Debug.LogError("No Orientation Controller atached to Player");
            }
        }

        if (movement == null) {

            movement = GetComponent<MovementController>();

            if (movement == null) {
                Debug.LogError("No Movement Controller atached to Player");
            }
        }

        if (anim == null) {

            anim = GetComponent<AnimationController>();

            if (anim == null) {
                Debug.LogError("No Animation Controller atached to Player");
            }
        }

        if (attackZone == null) {

            attackZone = GetComponentInChildren<AttackZone>();

            if (attackZone == null) {
                Debug.LogError("No Attack Zone attached!");
            }
        }

    }

    private void Start() {

        health = maxHealth;

        if (attackZone != null) {
            attackZone.SetDamage(attackDamage);
        }
    }

    private void FixedUpdate() {

        HandleStates();
    }

    public void TakeDamage(float damage) {

        health--;

        if (health <= 0) {
            currState = State.Dead;
        }
    }

    // activate method corresponding to current state
    private void HandleStates() {
        
        switch (currState) {

            case State.Idle:
                Idle();
            break;

            case State.Moving:
                Move();
            break;

            case State.Attacking:
                Attack();
            break;

            case State.Dead:
                Die();
            break;
        }
    }

    private void Idle() {

        // transition to attack if target is in range and it is not in cooldown
        if (attackDetectionZone.HasTarget && canAttack) {
            anim.SetAnimatorBool(attackingAnimParam, true); 
            StartCoroutine(AttackAnimationTimer());
            currState = State.Attacking;
            return;
        }

        // if character is in detection zone and outside of stop distance, move to it
        if (detectionZone.Target != null) {
            
            if (!movement.ReachedDestination(detectionZone.Target.transform.position)) {
                anim.SetAnimatorBool(movingAnimParam, true); 
                currState = State.Moving;
            }
        }
        
    }

    private void Move() {

        // transition to attack if target is in range and it is not in cooldown
        if (attackDetectionZone.HasTarget && canAttack) {
            anim.SetAnimatorBool(movingAnimParam, false);
            anim.SetAnimatorBool(attackingAnimParam, true); 
            StartCoroutine(AttackAnimationTimer());
            currState = State.Attacking;
            return;
        }

        // change target position if target is still in zone
        if (detectionZone.Target != null) {
            targetPos = detectionZone.Target.transform.position;
        }

        // if zombie has reached target position stop
        if (movement.ReachedDestination(detectionZone.Target.transform.position)) {

            anim.SetAnimatorBool(movingAnimParam, false);
            currState = State.Idle;
            targetPos = Vector2.zero;
            return;
        }

        // move to target
        movement.SetDirection((targetPos - transform.position).normalized);
    }

    // attack state; wait for can attack to be set to false than return to false
    protected void Attack() {
        
        if (!canAttack) {
            currState = State.Idle;
            StartCoroutine(AttackCooldown());
        }
    }

    // stop attack animation after the specified time
    private IEnumerator AttackAnimationTimer() {

        yield return new WaitForSeconds(attackAnimTime);
        anim.SetAnimatorBool(attackingAnimParam, false);
        canAttack = false;
        AttackCooldown();
    }

    // handle attack cooldown
    private IEnumerator AttackCooldown() {

        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }

    protected void Die() {
        anim.SetAnimatorBool(deathAnimParam, true);
    }
}
