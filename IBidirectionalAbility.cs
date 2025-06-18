public interface IBidirectionalAbility : ITimedAbility
{
    public Ability GetReturnAbility();
    public void SetReturnAbility(Ability returnAbility);
}
