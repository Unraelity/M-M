using UnityEngine;

// ability that allow character to move up
public class MoveRightAbility : MoveAbility {

    public MoveRightAbility(MovementController movementController, AnimationController animController) : base(movementController, animController) {}


    public override void ExecuteAbility() {

        Vector2 dir = new Vector2(1, 0);
        _movementController.SetDirection(dir);
    }

}
