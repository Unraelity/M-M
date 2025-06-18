using UnityEngine;

// ability that allow character to move up
public class RollLeftAbility : RollAbility {

    public RollLeftAbility(PlayerController playerController, OrientationController orientationController, MovementController movementController, AnimationController animController, float rollDuration, float rollTiles) : base(playerController, orientationController, movementController, animController, rollDuration, rollTiles) {}

    public override void EnterAbility()
    {
        if (_isRolling)
        {
            return;
        }
        
        _isRolling = true;
        _animController.SetAnimatorBool(_animParameter, true);

        Vector2 dir = new Vector2(-1, 0);

        Roll(dir);
    }

}
