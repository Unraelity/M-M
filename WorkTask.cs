using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class WorkTask : ITask {

    private VillagerController controller;
    private float offWorkTime;
    private bool isRunning = false;

    public WorkTask(VillagerController controller, float offWorkTime) {
        this.controller = controller;
        this.offWorkTime = offWorkTime;
    }

    public bool IsExecutable(WorldState worldState) {
        
        if (controller != null && worldState.Weather == WeatherManager.WeatherStates.Sunny) {
            return true;
        }

        return false;
    }

    public void Execute(WorldState worldState) {
        if (!isRunning) {
            
            isRunning = true;
            controller.EnterMovementState(CharacterStates.MovementStates.Working);
        }
    }

    public bool IsComplete(WorldState worldState) {

        if (isRunning && (worldState.Time >= offWorkTime)) {
            isRunning = false;
            return true;
        }

        return false;
    }

    public bool IsRunning() {
        return isRunning;
    }
}
