using UnityEngine;

// ability that allow character to move up
public class RollLeftUpAbility : RollAbility {

    public RollLeftUpAbility(PlayerController playerController, OrientationController orientationController, MovementController movementController, AnimationController animController, float rollDuration, float rollTiles) : base(playerController, orientationController, movementController, animController, rollDuration, rollTiles) {}

    public override void EnterAbility()
    {
        if (_isRolling)
        {
            return;
        }
        
        _isRolling = true;
        _animController.SetAnimatorBool(_animParameter, true);

        Vector2 dir = new Vector2(-1, 1).normalized;

        Roll(dir);
    }

}
