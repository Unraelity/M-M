using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class SetPathTask : ITask {

    private AIController controller;
    private string pathName;
    private bool isRunning;


    public SetPathTask(AIController controller, string pathName) {
        this.controller = controller;
        this.pathName = pathName;
        this.isRunning = false;
    }

    public bool IsExecutable(WorldState worldState) {

        if (controller) {
            return true;
        }

        return false;
    }

    public void Execute(WorldState worldState) {

        if (controller) {

            isRunning = true;
            controller.SetPath(pathName);
        }
    }

    public bool IsRunning() {
        return isRunning && !IsComplete(null);
    }

    public bool IsComplete(WorldState worldState) {

        if (controller.OnPath == false) {
            
            controller.SetPath("Null");
            Debug.Log("Path complete");
            isRunning = false;
            
            return true;
        }

        return false;
    }
}