using System.Collections.Generic;
using UnityEngine;

public class PathfindTask : ITask {

    private const float stoppingDistance = 0.01f;
    private const string animParam = "Walking";
    private const float updatePathTime = 1f;

    private bool isRunning = false;
    private bool isComplete = false;
    private MovementController movementController;
    private AnimationController animController;
    private PathfindingAgent pathfindingAgent;
    // on path variables
    private Vector2 endingTargetPos;
    // off path variables
    private List<Tile> offPathTiles;
    int offPathIndex = 0;
    private int offPathSize = 0;
    float updatePathTimer = 0f;

    // overloaded constructor that allows for creation of MoveTask from Vector2s
    public PathfindTask(MovementController movementController, AnimationController animController, PathfindingAgent pathfindingAgent, Vector2 endingTargetPos)
    {
        this.movementController = movementController;
        this.animController = animController;
        this.pathfindingAgent = pathfindingAgent;

        this.endingTargetPos = endingTargetPos;

        offPathTiles = pathfindingAgent.FindPath(endingTargetPos);

        if (offPathTiles == null)
        {
            NoPath();
            return;
        }
        offPathSize = offPathTiles.Count - 1;
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
        MoveOffPath();
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    // function for when NPC has switched off the path
    private void MoveOffPath()
    {
        updatePathTimer += Time.deltaTime;

        // if NPC is close to ending target position, stop
        if ((offPathTiles != null) && (Mathf.Abs(movementController.transform.position.x - offPathTiles[offPathTiles.Count - 1].Position.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - offPathTiles[offPathTiles.Count - 1].Position.y) < stoppingDistance))
        {
            EndPath();
        }
        
        // if arrived at next tile, get next one
        if ((offPathTiles == null) || (Mathf.Abs(movementController.transform.position.x - offPathTiles[offPathIndex].Position.x) < stoppingDistance) && (Mathf.Abs(movementController.transform.position.y - offPathTiles[offPathIndex].Position.y) < stoppingDistance))
        {
            // if time to update path, get new path
            if (updatePathTimer >= updatePathTime)
            {
                offPathTiles = pathfindingAgent.FindPath(endingTargetPos);

                if (offPathTiles != null)
                {
                    offPathIndex = 0;
                    offPathSize = offPathTiles.Count - 1;
                }

                updatePathTimer = 0f;
            }
            
            // if no path is found, wait for next iteration
            if (offPathTiles == null)
            {
                NoPath();
                return;
            }

            if (offPathIndex < offPathSize)
            {
                offPathIndex++;
            }
            else
            {
                EndPath();
            }
            return;
        }

        animController.SetAnimatorBool(animParam, true);
        Vector2 dir = new Vector2(offPathTiles[offPathIndex].Position.x - movementController.transform.position.x, offPathTiles[offPathIndex].Position.y - movementController.transform.position.y);

        isRunning = true;
        movementController.SetDirection(dir.normalized);
    }

    private void NoPath()
    {
        isRunning = true;
        animController.SetAnimatorBool(animParam, false);
        movementController.SetDirection(Vector2.zero);
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
