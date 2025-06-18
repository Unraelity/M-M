using UnityEngine;

// ability that allow character to move up
public class RollDownAbility : RollAbility {

    public RollDownAbility(PlayerController playerController, OrientationController orientationController, MovementController movementController, AnimationController animController, float rollDuration, float rollTiles) : base(playerController, orientationController, movementController, animController, rollDuration, rollTiles) {}

    public override void EnterAbility()
    {
        if (_isRolling)
        {
            return;
        }
        
        _isRolling = true;
        _animController.SetAnimatorBool(_animParameter, true);

        Vector2 dir = new Vector2(0, -1);

        Roll(dir);
    }

}
