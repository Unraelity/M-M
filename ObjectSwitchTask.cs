
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ObjectSwitchTask : ITask {

    private ObjectSwitch targetObject;
    private bool isRunning = false;
    private bool turnOn;  // New field to determine if turning on or off

    public ObjectSwitchTask(ObjectSwitch targetObject, bool turnOn) {
        this.targetObject = targetObject;
        this.turnOn = turnOn;
    }
    
    public bool IsExecutable(WorldState worldState) {
        return true;
    }

    public void Execute(WorldState worldState) {
        if (!isRunning) {
            if (turnOn) {
                targetObject.TurnOn();
            } else {
                targetObject.TurnOff();
            }
            isRunning = true;
        }
    }

    public bool IsComplete(WorldState worldState) {

        if (isRunning && ((turnOn && targetObject.CurrentState == ObjectSwitch.SwitchState.On) ||
                          (!turnOn && targetObject.CurrentState == ObjectSwitch.SwitchState.Off))) {
            isRunning = false;
            return true;
        }
        return false;
    }

    public bool IsRunning() {
        return isRunning;
    }
}