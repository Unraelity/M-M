using UnityEngine;

// primitive class for moving
public class MoveToTask : ITask {

    private const float stoppingDistance = 0.01f;
    private const string animParam = "Walking";

    private MovementController movementController;
    private AnimationController animController;
    private Vector2 targetPos;
    private bool isRunning = false;
    private bool isComplete = false;

    public MoveToTask(MovementController movementController, AnimationController animController, Vector2 targetPos)
    {
        this.movementController = movementController;
        this.animController = animController;
        this.targetPos = targetPos;
    }

    public bool CanExecute(WorldState worldState)
    {
        //Debug.Log("Target Pos: " + targetPos);
        //Debug.Log("Current Pos: " + controller.transform.position);
        //Debug.Log("DeltaX: " + (controller.transform.position.x - targetPos.x));
        //Debug.Log("DeltaY: " + (controller.transform.position.y - targetPos.x));

        // if close to target position
        if ((Mathf.Abs(movementController.transform.position.x - targetPos.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - targetPos.y) < stoppingDistance)) {
            return false;
        }

        return true;
    }

    // on execute set direction in controller
    public void Execute(WorldState worldState)
    {
        if (worldState.Get<bool>(WorldLabels.ObstacleDetection))
        {

            animController.SetAnimatorBool(animParam, false);
            movementController.SetDirection(Vector2.zero);
            return;
        }

        animController.SetAnimatorBool(animParam, true);

        // if close to target position, stop
        if ((Mathf.Abs(movementController.transform.position.x - targetPos.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - targetPos.y) < stoppingDistance)) {

            animController.SetAnimatorBool(animParam, false);
            movementController.SetDirection(Vector2.zero);
            
            // clamp current position to target position
            movementController.transform.position = targetPos;

            isComplete = true;
            isRunning = false;
            return;
        }

        Vector2 dir = new Vector2(targetPos.x - movementController.transform.position.x, targetPos.y - movementController.transform.position.y);

        isRunning = true;
        movementController.SetDirection(dir.normalized);
    }

    public bool IsComplete()
    {
        return isComplete;
    } 

    public bool IsRunning()
    {
        return isRunning;
    }
}