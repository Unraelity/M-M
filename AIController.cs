using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using Pathfinding;

namespace MoreMountains.TopDownEngine {
    public class AIController : TopDownController2D {

        [SerializeField] private float stopDistance = 0.1f; // Distance at which the character stops moving

        public UnityEngine.Vector3 TargetPos;
        public bool OnPath = false;

        protected Character _character;
        public MMStateMachine<CharacterStates.MovementStates> Movement;
		public MMStateMachine<CharacterStates.CharacterConditions> Condition;

        // Dictionary to store paths by their name
        protected Dictionary<string, Route> _pathsDictionary;

        private CharacterMovement characterMovement;
        private AIPath aiPath;
        protected UnityEngine.Vector3[] _currPath = null;
        protected int _pathWayPointIndex = -1;

        protected override void Awake() {
            base.Awake();

            // Get the Character component
            _character = GetComponent<Character>();

            // Get the AIPath component
            aiPath = GetComponent<AIPath>();
        }

        protected virtual void Start() {

            // Get the condition state machine
            Condition = _character.ConditionState;

            // Get the movement state machine
            Movement = _character.MovementState;

            // Get the CharacterMovement ability
            characterMovement = _character.FindAbility<CharacterMovement>();

            // Initialize the dictionary
            _pathsDictionary = new Dictionary<string, Route>();

            // Add null route to dictionary
            _pathsDictionary.Add("Null", null);

            // Ensure the character movement is driven by script
            if (characterMovement != null) {
                characterMovement.ScriptDrivenInput = true;
            }
        }

        protected override void Update() {

            base.Update();

            if (_currPath != null) {
                ControlPath();
            }
            else {
                ControlMovement();
            }
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
        }

        protected override void LateUpdate() {
            base.LateUpdate();
        }

        public void SetPath(string PathName) {

            OnPath = true;
            _pathWayPointIndex = -1;

            Route newPath = FindPathByName(PathName);

            if (newPath != null) {
                _currPath = newPath.Waypoints;
            }
        }

        private void ControlMovement() {

            if (TargetPos != UnityEngine.Vector3.zero) {

                // Check if the character is close enough to the destination
                if (UnityEngine.Vector3.Distance(transform.position, TargetPos) < stopDistance) {

                    characterMovement.SetMovement(UnityEngine.Vector2.zero); // Stop moving
                    TargetPos = UnityEngine.Vector3.zero;
                    aiPath.isStopped = true; // Optionally stop the AIPath movement


                    return;
                }

                aiPath.maxSpeed = characterMovement.MovementSpeed;

                // Set the AIPath destination to the target's position
                aiPath.destination = TargetPos;

                // Update the character movement based on AIPath's direction
                if (characterMovement != null && aiPath.hasPath) {
                    UnityEngine.Vector2 direction = new UnityEngine.Vector2(aiPath.steeringTarget.x - transform.position.x, aiPath.steeringTarget.y - transform.position.y).normalized;
                    characterMovement.SetMovement(direction);
                }
            }
        }

        private void ControlPath() {
            
            if (_pathWayPointIndex == -1) {
                _pathWayPointIndex++;
                MoveToNextWaypoint(_currPath);
            } 
            else {
                if (_pathWayPointIndex < _currPath.Length) {

                    float distanceToNextPoint = UnityEngine.Vector3.Distance(transform.position, _currPath[_pathWayPointIndex]);

                    if (distanceToNextPoint <= stopDistance) {
                        _pathWayPointIndex++;
                        if (_pathWayPointIndex < _currPath.Length) {
                            MoveToNextWaypoint(_currPath);
                        } 
                        else {
                            characterMovement.SetMovement(UnityEngine.Vector2.zero); // Stop at the end of the path
                            OnPath = false;
                        }
                    }
                } else {
                    characterMovement.SetMovement(UnityEngine.Vector2.zero); // Stop at the end of the path
                    OnPath = false;
                }
            }
        }

        private void MoveToNextWaypoint(UnityEngine.Vector3[] pathArray) {

            if (_pathWayPointIndex >= 0 && _pathWayPointIndex < pathArray.Length) {
                UnityEngine.Vector2 direction = new UnityEngine.Vector2(pathArray[_pathWayPointIndex].x - transform.position.x, pathArray[_pathWayPointIndex].y - transform.position.y).normalized;
                characterMovement.SetMovement(direction);
            }
        }

        protected virtual void StateControl() {
        }

        public virtual void EnterCharacterCondition(CharacterStates.CharacterConditions nextCharacterCondition) {

        }

        public virtual void EnterMovementState(CharacterStates.MovementStates nextMovementState) {

        }

        public virtual void ExitMovementState() {
        }

        public virtual void ExitMovementState(CharacterStates.MovementStates exitMovementState) {
        }

        // Function to find a path by name using the dictionary
        protected Route FindPathByName(string routeName) {

            if (_pathsDictionary.TryGetValue(routeName, out Route route)) {
                return route;
            } 
            else {
                Debug.LogWarning("Path not found: " + routeName);
                return null;
            }
        }

    }
}