using UnityEditor.Animations;
using UnityEngine;

// ability that controls movement
public class MoveAbility : Ability {

    private AnimationController animatorController;
    protected MovementController _movementController;

    public MoveAbility(MovementController movementController, AnimationController animatorController) {

        _movementController = movementController;
        this.animatorController = animatorController;

        _animParameter = "Walking";
    }

    public override void EnterAbility() {
        animatorController.SetAnimatorBool(_animParameter, true);
    }

    public override void ExecuteAbility() {}


    public override void ExitAbility() {
        
        animatorController.SetAnimatorBool(_animParameter, false);
        _movementController.SetDirection(Vector2.zero);
    }
}
