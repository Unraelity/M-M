using UnityEngine;

// ability that allow character to move up
public class MoveLeftAbility : MoveAbility {

    public MoveLeftAbility(MovementController movementController, AnimationController animController) : base(movementController, animController) {}


    public override void ExecuteAbility() {

        Vector2 dir = new Vector2(-1, 0);
        _movementController.SetDirection(dir);
    }

}
