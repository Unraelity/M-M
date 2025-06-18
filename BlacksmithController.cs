using UnityEngine;

public class BlacksmithController : BaseController {

    [Header("Component Settings")]
    [SerializeField] private OrientationController orientationController;
    [SerializeField] private MovementController movementController;
    [SerializeField] private AnimationController animController;
    [Header("AStar Agent")]
    [SerializeField] private PathfindingAgent pathfindingAgent;
    [SerializeField] private ObstacleDetection obstacleDetection;

    private WorldState worldState;
    private ITask currTask;
    private BlacksmithPlanner planner;
    private WeatherManager weatherManager;

    private void Awake()
    {
        if (orientationController == null)
        {
            orientationController = GetComponent<OrientationController>();

            if (orientationController == null)
            {
                Debug.LogError("No Orientation Controller atached to Blacksmith");
            }
        }

        if (movementController == null)
        {
            movementController = GetComponent<MovementController>();

            if (movementController == null)
            {
                Debug.LogError("No Movement Controller atached to Blacksmith");
            }
        }

        if (animController == null)
        {
            animController = GetComponent<AnimationController>();

            if (animController == null)
            {
                Debug.LogError("No Animation Controller atached to Blacksmith");
            }
        }

        if (pathfindingAgent == null)
        {
            pathfindingAgent = GetComponent<PathfindingAgent>();

            if (pathfindingAgent == null)
            {
                Debug.LogError("No Pathfinding Agent atached to Blacksmith");
            }
        }

        if (obstacleDetection == null)
        {
            obstacleDetection = GetComponentInChildren<ObstacleDetection>();

            if (obstacleDetection == null)
            {
                Debug.LogError("No Obstacle Detection atached to Blacksmith Children Objects");
            }
        }
    }

    private void Start()
    {
        weatherManager = WeatherManager.Instance;

        planner = new BlacksmithPlanner(movementController, animController, pathfindingAgent);

        // world state setup
        worldState = new WorldState();
        worldState.Set(WorldLabels.Time, ClockManager.Instance.CurrTime);
        worldState.Set(WorldLabels.ObstacleDetection, obstacleDetection.HasTargetsInRange);
        worldState.Set(WorldLabels.Weather, weatherManager.CurrWeatherType);
    }

    private void Update()
    {
        UpdateWorldState();

        if (currTask == null)
        {
            currTask = planner.GetTask();
        }

        // if current task is not running, check if you can execute it and execute it if so
        if (!currTask.IsRunning() && !currTask.IsComplete())
        {
            if (currTask.CanExecute(worldState))
            {
                //Debug.Log("Executing " + currTask);
                currTask.Execute(worldState);
            }
            else
            {
                //Debug.Log("Cannot execute " + currTask);
                currTask = planner.GetTask();
            }
        }

        // if current task is not complete, execute is
        if (!currTask.IsComplete())
        {
            currTask.Execute(worldState);
        }
        // if current task is complete and compound, get children
        else {
            //Debug.Log(currTask + " is complete");
            if (currTask is ICompositeTask compoundTask)
            {
                planner.AddListToTaskStack(compoundTask.Decompose(worldState));
            }
            
            currTask = planner.GetTask();
        }
    }

    private void UpdateWorldState()
    {
        worldState.Set(WorldLabels.Time, ClockManager.Instance.CurrTime);
        worldState.Set(WorldLabels.ObstacleDetection, obstacleDetection.HasTargetsInRange);
        worldState.Set(WorldLabels.Weather, weatherManager.CurrWeatherType);
    }
}
