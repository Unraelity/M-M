using UnityEngine;
public class RollAbility : Ability, ITimedAbility {

    private PlayerController playerController;
    private OrientationController orientationController;
    private MovementController movementController;
    protected AnimationController _animController;
    protected bool _isRolling;
    private float rollDuration = 0.5f;
    private float rollTiles = 1.5625f;
    private float elapsedTime;
    private Vector3 startPos;
    private Vector2 targetPos;

    public RollAbility(PlayerController playerController, OrientationController orientationController, MovementController movementController, AnimationController animController, float rollDuration, float rollTiles)
    {
        this.playerController = playerController;
        this.orientationController = orientationController;
        this.movementController = movementController;
        _animController = animController;

        this.rollDuration = rollDuration;
        this.rollTiles = rollTiles;
        _isRolling = false;

        _animParameter = "Rolling";
    }

    public bool IsComplete() {
        return !_isRolling;
    }

    public override void EnterAbility()
    {
        if (_isRolling)
        {
            return;
        }
        _isRolling = true;
        _animController.SetAnimatorBool(_animParameter, true);
        playerController.HitBox.SetActive(false);
        movementController.SetDirection(Vector2.zero);

        Vector2 dir;

        if (orientationController.FacingWest) {
            dir = new Vector2(1, 0);
        }
        else {
            dir = new Vector2(-1, 0);
        }

        Roll(dir);
    }

    public override void ExecuteAbility()
    {
        if (elapsedTime < rollDuration)
        {
            elapsedTime += Time.deltaTime;
            movementController.MovePosition(Vector3.Lerp(startPos, targetPos, elapsedTime / rollDuration));
        }
        else {
            ExitAbility();
        }
    }

    public override void ExitAbility()
    {
        _isRolling = false;
        _animController.SetAnimatorBool(_animParameter, false);
        playerController.HitBox.SetActive(true);
        movementController.SetDirection(Vector2.zero);
    }

    protected void Roll(Vector2 dir)
    {
        // roll 3 tiles in the direction character is moving or facing
        startPos = playerController.transform.position;
        targetPos = (Vector2)playerController.transform.position + (dir * rollTiles);

        elapsedTime = 0f;
    }
}
