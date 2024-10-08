using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace MoreMountains.TopDownEngine {
    public class VillagerController : AIController {

        [SerializeField] private UnityEngine.Vector3[] homeToWorkPath = new UnityEngine.Vector3[0];
        [SerializeField] private UnityEngine.Vector3[] workToHomePath = new UnityEngine.Vector3[0];
        [SerializeField] protected float _wakeUpTime = 360f;
        [SerializeField] protected float _offWorkTime = 1020f;
        
        protected WorldState _worldState;

        protected string _homeToWorkPathName = "Home To Work";
        protected string _workToHomePathName = "Work To Home";

        private Route homeToWorkRoute;
        private Route workToHomeRoute;

        private CharacterWork characterWork;

        protected override void Awake() {
            base.Awake();
        }

        protected override void Start() {

            base.Start();

            _worldState = new WorldState();

            // Get the CharacterWork ability
            characterWork = _character.FindAbility<CharacterWork>();

            // Create and add paths to the dictionary
            homeToWorkRoute = new Route(_homeToWorkPathName, homeToWorkPath);
            workToHomeRoute = new Route(_workToHomePathName, workToHomePath);

            _pathsDictionary.Add(homeToWorkRoute.Name, homeToWorkRoute);
            _pathsDictionary.Add(workToHomeRoute.Name, workToHomeRoute);
        }

        protected override void Update() {
            base.Update();

            StateControl();
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
        }

        protected override void LateUpdate() {
            base.LateUpdate();
        }

        protected override void StateControl() {

            switch(Condition.CurrentState) {

                case CharacterStates.CharacterConditions.Normal:

                    break;

                case CharacterStates.CharacterConditions.Fleeing:
                    
                    break;

                case CharacterStates.CharacterConditions.Paused:
                    
                    break;

                case CharacterStates.CharacterConditions.Dead:
                    
                    break;
            }
        } 

        public override void EnterCharacterCondition(CharacterStates.CharacterConditions nextCharacterCondition) {

            if (nextCharacterCondition == Condition.CurrentState) {
                return;
            } 

            switch(nextCharacterCondition) {
                case CharacterStates.CharacterConditions.Normal:
                    Condition.ChangeState(CharacterStates.CharacterConditions.Normal);
                    break;
                case CharacterStates.CharacterConditions.Fleeing:
                    Condition.ChangeState(CharacterStates.CharacterConditions.Fleeing);
                    break;
            }
        }

        public override void EnterMovementState(CharacterStates.MovementStates nextMovementState) {

            if (nextMovementState == Movement.CurrentState) {
                return;
            }

            if (Movement.CurrentState != CharacterStates.MovementStates.Idle) {
                ExitMovementState(Movement.CurrentState);
            }

            switch(nextMovementState) {
                case CharacterStates.MovementStates.Idle:
                    break;
                case CharacterStates.MovementStates.Working:
                    characterWork.WorkStart();
                    break;
                case CharacterStates.MovementStates.Sleeping:
                    //characterWork.SleepStart();
                    break;
                case CharacterStates.MovementStates.Eating:
                    //characterWork.EatStart();
                    break;
            }
        }

        public override void ExitMovementState() {

            switch(Movement.CurrentState) {
                case CharacterStates.MovementStates.Idle:
                    break;
                case CharacterStates.MovementStates.Working:
                    characterWork.WorkStop();
                    break;
                case CharacterStates.MovementStates.Sleeping:
                    //characterWork.SleepStop();
                    break;
                case CharacterStates.MovementStates.Eating:
                    //characterWork.EatStop();
                    break;
            }
        }

        public override void ExitMovementState(CharacterStates.MovementStates exitMovementState) {

            switch(exitMovementState) {
                case CharacterStates.MovementStates.Idle:
                    break;
                case CharacterStates.MovementStates.Working:
                    characterWork.WorkStop();
                    break;
                case CharacterStates.MovementStates.Sleeping:
                    //characterWork.SleepStop();
                    break;
                case CharacterStates.MovementStates.Eating:
                    //characterWork.EatStop();
                    break;
            }
        }
    }
}