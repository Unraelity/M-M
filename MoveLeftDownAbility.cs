using UnityEngine;

// ability that allow character to move up
public class MoveLeftDownAbility : MoveAbility {

    public MoveLeftDownAbility(MovementController movementController, AnimationController animController) : base(movementController, animController) {}


    public override void ExecuteAbility() {

        Vector2 dir = new Vector2(-1, -1).normalized;
        _movementController.SetDirection(dir);
    }

}
