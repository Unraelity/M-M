using UnityEngine;

// ability that allow character to move up
public class MoveUpAbility : MoveAbility {

    public MoveUpAbility(MovementController movementController, AnimationController animController) : base(movementController, animController) {}

    public override void ExecuteAbility() {

        Vector2 dir = new Vector2(0, 1);
        _movementController.SetDirection(dir);
    }

}
