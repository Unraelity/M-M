using System.Collections.Generic;
using UnityEngine;

// compound task that walks character along a path
public class MoveAlongPathTask : ICompositeTask
{
    private const float stoppingDistance = 0.01f;
    private const string animParam = "Walking";

    private bool isRunning = false;
    private bool isComplete = false;
    private MovementController movementController;
    private AnimationController animController;
    private PathfindingAgent pathfindingAgent;
    // on path variables
    private Vector2[] path;
    private int pathSize = 0;
    private Vector2 endingTargetPos;
    int currIndex = 0;
    private Vector2 currTargetPos;
    // off path variables
    private bool offPath;
    private List<ITask> children;

    // overloaded constructor that allows for creation of MoveTask from Vector2s
    public MoveAlongPathTask(MovementController movementController, AnimationController animController, PathfindingAgent pathfindingAgent, Vector2[] path)
    {

        children = new List<ITask>();

        this.movementController = movementController;
        this.animController = animController;
        this.pathfindingAgent = pathfindingAgent;

        this.path = path;
        pathSize = path.Length - 1;
        endingTargetPos = path[pathSize];
    }

    public bool CanExecute(WorldState worldState)
    {
        if ((Mathf.Abs(movementController.transform.position.x - endingTargetPos.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - endingTargetPos.y) < stoppingDistance))
        {
            return false;
        }

        return true;
    }

    public void Execute(WorldState worldState)
    {
        if (currIndex <= pathSize)
        {
            MoveToCurrTargetPosition(worldState);
        }
        else
        {
            isComplete = true;
            isRunning = false;
        }
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public List<ITask> Decompose(WorldState worldState)
    {
        if (offPath)
        {
            if (path == null)
            {
                Debug.Log("Path is null");
            }
            if (pathfindingAgent == null)
            {
                Debug.Log("Pathfinding agent is null");
            }
            children.Add(new PathfindTask(movementController, animController, pathfindingAgent, path[pathSize]));
        }

        return children;
    }

    private void MoveToCurrTargetPosition(WorldState worldState)
    {
        animController.SetAnimatorBool(animParam, true);

        // if it detects obstacles, switch to off path pathfinding
        if (worldState.Get<bool>(WorldLabels.ObstacleDetection))
        {
            //Debug.Log("Switching off the path");
            offPath = true;
            isComplete = true;
            isRunning = false;
            return;
        }

        currTargetPos = path[currIndex];

        // if close to target position, either stop or move to next target
        if ((Mathf.Abs(movementController.transform.position.x - currTargetPos.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - currTargetPos.y) < stoppingDistance))
        {
            if (currIndex < pathSize)
            {
                currIndex++;
            }
            else
            {
                EndPath();
            }

            return;
        }

        Vector2 dir = new Vector2(currTargetPos.x - movementController.transform.position.x, currTargetPos.y - movementController.transform.position.y);

        isRunning = true;
        movementController.SetDirection(dir.normalized);
    }

    // function called once finished path
    private void EndPath()
    {
        animController.SetAnimatorBool(animParam, false);
        movementController.SetDirection(Vector2.zero);

        // clamp current position to end target position
        movementController.transform.position = endingTargetPos;

        isComplete = true;
        isRunning = false;
    }
} 
