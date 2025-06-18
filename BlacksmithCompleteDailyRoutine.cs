using System.Collections.Generic;
using UnityEngine;

// task that will pause NPC until a given clock interval
public class BlacksmithCompleteDailyRoutine : ICompositeTask {

    private const float workStartTime = 360f;
    private const float workEndTime = 1050f;

    private bool isRunning = false;
    private bool isComplete = false;
    private MovementController movementController;
    private AnimationController animController;
    private PathfindingAgent pathfindingAgent;
    
    private List<ITask> children;

    // pass through waiting interval in constructor
    public BlacksmithCompleteDailyRoutine(MovementController movementController, AnimationController animController, PathfindingAgent pathfindingAgent)
    {
        children = new List<ITask>();
        this.movementController = movementController;
        this.animController = animController;
        this.pathfindingAgent = pathfindingAgent;
    }

    public bool CanExecute(WorldState worldState) {
        return false;
    }

    public void Execute(WorldState worldState) {

        isComplete = true;
        isRunning = false;
    }

    public bool IsComplete() {
        return isComplete;
    } 

    public bool IsRunning() {
        return isRunning;
    }

    public List<ITask> Decompose(WorldState worldState) {

        float currTime = worldState.Get<float>(WorldLabels.Time);

        if ((workStartTime < currTime) && (currTime < workEndTime)) {

            Vector2[] path = {
                new Vector2(0f, 0f),
                new Vector2(0f, 3f),
                new Vector2(3f, 5f)
            };
            children.Add(new MoveAlongPathTask(movementController, animController, pathfindingAgent, path));
        }

        return children;
    }
}
