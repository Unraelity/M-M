public class BlacksmithPlanner : HTNPlanner {

    private MovementController movementController;
    private AnimationController animController;
    private PathfindingAgent pathfindingAgent;

    public BlacksmithPlanner(MovementController movementController, AnimationController animController, PathfindingAgent pathfindingAgent) : base()
    {

        this.movementController = movementController;
        this.animController = animController;
        this.pathfindingAgent = pathfindingAgent;
        Initialize();
    } 

    protected override void Initialize() {
        AddToTaskStack(new BlacksmithCompleteDailyRoutine(movementController, animController, pathfindingAgent));
    }
}
