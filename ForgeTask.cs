using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine {
    public class ForgeTask : ICompositeTask {

        private List<ITask> children;
        private bool isRunning = false;
        private string steelString = "Steel";

        private SetPathTask homeToForgePathTask;
        private SetPathTask forgeToHomePathTask;
        private WorkTask workTask;
        private ObjectSwitchTask activateBenchTask;
        private ObjectSwitchTask deactivateBenchTask;


        public ForgeTask(SetPathTask homeToForgePathTask, SetPathTask forgeToHomePathTask, WorkTask workTask, ObjectSwitchTask activateBenchTask, ObjectSwitchTask deactivateBenchTask) {
            children = new List<ITask>();
            
            this.homeToForgePathTask = homeToForgePathTask;
            this.forgeToHomePathTask = forgeToHomePathTask;
            this.workTask = workTask;
            this.activateBenchTask = activateBenchTask;
            this.deactivateBenchTask = deactivateBenchTask;

        }

        public bool IsExecutable(WorldState worldState) {

            return true;
            //return worldState.Weather == WorldState.WeatherType.Sunny 
                //&& worldState.Inventory.GetQuantity(steelString) >= 5;
        }

        public void Execute(WorldState worldState) {
            if (!isRunning) {
                Decompose();
                isRunning = true;
            }
        }

        public bool IsComplete(WorldState worldState) {
            if (isRunning) {
                isRunning = false;
                return true;
            }
            return false;
        }

        public bool IsRunning() {
            return isRunning;
        }

        public void Decompose() {

            AddChild(forgeToHomePathTask);
            AddChild(deactivateBenchTask);
            AddChild(workTask);
            AddChild(activateBenchTask);
            AddChild(homeToForgePathTask);
        }

        public void AddChild(ITask child) {
            children.Add(child);
        }

        public void RemoveChild(ITask child) {
            children.Remove(child);
        }

        public List<ITask> GetChildren() {
            return children;
        }
    }

}