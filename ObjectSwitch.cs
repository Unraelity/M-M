using UnityEngine;
using MoreMountains.Tools;

public class ObjectSwitch : MonoBehaviour {

    public enum SwitchState {
        On,
        Off
    }

    [Header("Bindings")]
    [Tooltip("The Animator component to control")]
    public Animator targetAnimator;

    [Header("State")]
    [Tooltip("The name of the bool parameter in the Animator")]
    public string boolParameterName = "On";

    [Tooltip("The current state of the bool parameter")]
    public SwitchState CurrentState = SwitchState.Off;

    [MMInspectorButton("TurnOn")]
    public bool OnButton;

    [MMInspectorButton("TurnOff")]
    public bool OffButton;

    [MMInspectorButton("ToggleBool")]
    public bool ToggleButton;

    /// <summary>
    /// Turns the bool parameter in the Animator to true and updates the state
    /// </summary>
    public virtual void TurnOn() {

        if (targetAnimator != null) {

            targetAnimator.SetBool(boolParameterName, true);
            CurrentState = SwitchState.On;
        }
    }

    /// <summary>
    /// Turns the bool parameter in the Animator to false and updates the state
    /// </summary>
    public virtual void TurnOff() {

        if (targetAnimator != null) {

            targetAnimator.SetBool(boolParameterName, false);
            CurrentState = SwitchState.Off;
        }
    }

    /// <summary>
    /// Toggles the bool parameter in the Animator and updates the state
    /// </summary>
    public virtual void ToggleBool() {
        
        if (targetAnimator != null) {

            bool currentBoolState = targetAnimator.GetBool(boolParameterName);
            targetAnimator.SetBool(boolParameterName, !currentBoolState);

            if (currentBoolState) {
                CurrentState = SwitchState.Off;
            } else {
                CurrentState = SwitchState.On;
            }

        }
    }
}
