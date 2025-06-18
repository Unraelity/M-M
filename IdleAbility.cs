using UnityEngine;

public class IdleAbility : Ability {

    MovementController movementController;

    public IdleAbility(MovementController movementController) {
        this.movementController = movementController;
    }

    public override void EnterAbility() {
        movementController.SetDirection(Vector2.zero);
    }

    public override void ExecuteAbility() {}

    public override void ExitAbility() {}

}
