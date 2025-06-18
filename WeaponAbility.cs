using UnityEngine;

public class WeaponAbility : Ability, ITimedAbility {

    private PlayerController playerController;
    private AnimationController animController;
    private bool isAttacking;
    private float attackTimer = 0f;

    public WeaponAbility(PlayerController playerController, AnimationController animController)
    {
        this.playerController = playerController;
        this.animController = animController;
        isAttacking = false;
    }

    public bool IsComplete() {
        return !isAttacking;
    }

    // use attack of current weapon and set trigger of animation paramater returned
    public override void EnterAbility() {

        Weapon weapon = playerController.EquippedWeapon;
        if (weapon != null)
        {
            if (!weapon.CanAttack())
            {
                //Debug.Log("Weapon Cannot Attack!");
                return;
            }

            weapon.Attack();
            animController.SetAnimatorTrigger(weapon.AnimParam);

            attackTimer = weapon.AnimExitTime;
            isAttacking = true;
        }
        else
        {
            Debug.Log("No Weapon Attached!");
        }
    }

    public override void ExecuteAbility() {

        if (!isAttacking) {
            return;
        } 

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f) {
            isAttacking = false;
        }
    }

    public override void ExitAbility() {}
}
