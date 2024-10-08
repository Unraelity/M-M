using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine {
    public class BlacksmithController : VillagerController {

        [SerializeField] private ObjectSwitch objectSwitch;
        
        private BlacksmithPlanner planner;
        private ITask currentTaskNode;

        protected override void Awake() {
            base.Awake();
        }

        protected override void Start() {
            base.Start();

            planner = new BlacksmithPlanner(this, _worldState, objectSwitch);

            planner.SetPathNames(_homeToWorkPathName, _workToHomePathName);
            planner.SetTimes(_wakeUpTime, _offWorkTime);
            planner.SetupTaskTree();
            currentTaskNode = planner.Execute();
        }

        protected override void Update() {
            base.Update();

            StateControl();

            if (currentTaskNode != null && !currentTaskNode.IsRunning()) {
                currentTaskNode = planner.Execute();
            }
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
        }

        protected override void LateUpdate() {
            base.LateUpdate();
        }

    }
}