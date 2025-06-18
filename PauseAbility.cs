public class PauseAbility : Ability, IBidirectionalAbility
{
    private UIManager uIManager;
    private Ability returnAbility;

    public PauseAbility()
    {
        uIManager = UIManager.Instance;
    }

    public bool IsComplete()
    {
        return !uIManager.Paused;
    }

    public override void EnterAbility()
    {
        uIManager.Pause();
    }

    public override void ExecuteAbility() { }

    public override void ExitAbility() { }

    public Ability GetReturnAbility() => returnAbility;
    public void SetReturnAbility(Ability returnAbility)
    {
        this.returnAbility = returnAbility;
    }
}
