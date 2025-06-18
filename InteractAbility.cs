public class InteractAbility : Ability,  ITimedAbility
{
    
    private InteractionZone interactionZone;

    public InteractAbility(InteractionZone interactionZone)
    {
        this.interactionZone = interactionZone;
    }

    public bool IsComplete() {
        return true;
    }

    public override void EnterAbility()
    {
        if ((interactionZone != null) && (interactionZone.Target != null))
        {
            interactionZone.Target.Interact();
        }
    }

    public override void ExecuteAbility() {}

    public override void ExitAbility() {}
}
