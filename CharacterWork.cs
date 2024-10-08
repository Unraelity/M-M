using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// Add this ability to a character and it'll be able to work. This means no actual movement, only the collider turned off and on. Movement will be handled by the animation itself.
	/// </summary>
	[AddComponentMenu("TopDown Engine/Character/Abilities/Character Work")]
	public class CharacterWork : CharacterAbility 
	{
		public MMFeedbacks WorkStartFeedback;
		/// the feedback to play when the work stops
		[Tooltip("the feedback to play when the work stops")]
		public MMFeedbacks WorkStopFeedback;

		protected CharacterButtonActivation _characterButtonActivation;
		protected bool _buttonReleased = false;
        protected bool _workStopped = false;
		protected const string _workingAnimationParameterName = "Working";
		protected int _workingAnimationParameter;
		protected int _hitTheGroundAnimationParameter;

		/// <summary>
		/// On init we grab our components
		/// </summary>
		protected override void Initialization()
		{
			base.Initialization ();
			_characterButtonActivation = _character?.FindAbility<CharacterButtonActivation> ();
			WorkStartFeedback?.Initialization(this.gameObject);
			WorkStopFeedback?.Initialization(this.gameObject);
		}

		/// <summary>
		/// On HandleInput we watch for work input and trigger a work if needed
		/// </summary>
		protected override void HandleInput()
		{
			base.HandleInput();

            /*
			// if movement is prevented, or if the character is dead/frozen/can't move, we exit and do nothing
			if (!AbilityAuthorized
			    || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal)) {
				return;
			}
			if (_inputManager.WorkButton.State.CurrentState == MMInput.ButtonStates.ButtonDown) {
				WorkStart();
			}
			if (_inputManager.WorkButton.State.CurrentState == MMInput.ButtonStates.ButtonUp) {
				_buttonReleased = true;
			}
            */
		}

		/// <summary>
		/// On process ability, we stop the work if needed
		/// </summary>
		public override void ProcessAbility() {

			if (_movement.CurrentState == CharacterStates.MovementStates.Working) {
				if (!_workStopped) {
					_movement.ChangeState(CharacterStates.MovementStates.Working);
				}
			}
            /*
			if (_buttonReleased && !_workStopped)
				{
					WorkStop();
				}
            */
		}

		/// <summary>
		/// Starts the work
		/// </summary>
		public virtual void WorkStart() {

			/*
            if (!EvaluateConditions()) {
				return;
			}
            */
			_movement.ChangeState(CharacterStates.MovementStates.Working);	
			//MMCharacterEvent.Trigger(_character, MMCharacterEventTypes.Work);
			WorkStartFeedback?.PlayFeedbacks(this.transform.position);
			PlayAbilityStartFeedbacks();

			_buttonReleased = false;
		}

		/// <summary>
		/// Stops the work
		/// </summary>
		public virtual void WorkStop()
		{
			_workStopped = true;
			_movement.ChangeState(CharacterStates.MovementStates.Idle);
			_buttonReleased = false;
			WorkStopFeedback?.PlayFeedbacks(this.transform.position);
			StopStartFeedbacks();
			PlayAbilityStopFeedbacks();
		}

		/// <summary>
		/// Adds required animator parameters to the animator parameters list if they exist
		/// </summary>
		protected override void InitializeAnimatorParameters() {
			RegisterAnimatorParameter (_workingAnimationParameterName, AnimatorControllerParameterType.Bool, out _workingAnimationParameter);
		}

		/// <summary>
		/// At the end of each cycle, sends Working states to the Character's animator
		/// </summary>
		public override void UpdateAnimator() {
			MMAnimatorExtensions.UpdateAnimatorBool(_animator, _workingAnimationParameter, (_movement.CurrentState == CharacterStates.MovementStates.Working),_character._animatorParameters, _character.RunAnimatorSanityChecks);
		}
	}
}