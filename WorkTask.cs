public class WorkTask : ITask
{
    private const string animParam = "Working";

    private AnimationController animController;
    private bool isRunning = false;
    private bool isComplete = false;
    private float clockIn = 540f;
    private float clockOut = 1020f;
    private bool invertedTimeInterval = false;

    public WorkTask(AnimationController animController)
    {
        this.animController = animController;
    }

    public virtual bool CanExecute(WorldState worldState)
    {
        if (clockIn > clockOut)
        {
            invertedTimeInterval = true;
        }

        if (WithinTimeInterval(worldState.Get<float>(WorldLabels.Time)))
        {
            return true;
        }

        return false;
    }

    // on execute set direction in controller
    public void Execute(WorldState worldState)
    {
        animController.SetAnimatorBool(animParam, true);

        // if after clock out time
        if (!WithinTimeInterval(worldState.Get<float>(WorldLabels.Time)))
        {
            animController.SetAnimatorBool(animParam, false);

            isComplete = true;
            isRunning = false;
            return;
        }

        isRunning = true;
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    protected bool WithinTimeInterval(float currTime)
    {
        if (!invertedTimeInterval)
        {
            if ((currTime > clockIn) && (currTime < clockOut))
            {
                return true;
            }
        }
        else
        {
            if ((currTime < clockIn) && (currTime > clockOut))
            {
                return true;
            }
        }

        return false;
    }
}
