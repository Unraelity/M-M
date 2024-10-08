using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine {
    public class BlacksmithCompleteDailyRoutineTask : ICompositeTask {

        private BlacksmithController controller;
        private float wakeUpTime;
        private List<ITask> children;
        private bool isRunning = false;

        private ForgeTask forgeTask;

        public BlacksmithCompleteDailyRoutineTask(BlacksmithController controller, float wakeUpTime, ForgeTask forgeTask) {
            children = new List<ITask>();
            
            this.controller = controller;
            this.wakeUpTime = wakeUpTime;

            this.forgeTask = forgeTask;
        }

        public bool IsExecutable(WorldState worldState) {
            float currentTime = worldState.Time;
            return currentTime >= wakeUpTime;
        }

        public void Execute(WorldState worldState) {
            if (!isRunning) {
                Decompose();
                controller.ExitMovementState(CharacterStates.MovementStates.Sleeping);
                controller.EnterMovementState(CharacterStates.MovementStates.Idle);
                isRunning = true;
            }
        }

        public bool IsRunning() {
            return isRunning;
        }

        public bool IsComplete(WorldState worldState) {
            if (isRunning && controller.Movement.CurrentState == CharacterStates.MovementStates.Idle) {
                isRunning = false;
                return true;
            }
            return false;
        }

        public void Decompose() {
            AddChild(forgeTask);
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