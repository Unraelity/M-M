using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using System.Threading.Tasks;
using UnityEngine.Rendering;

public class BlacksmithPlanner : Planner {
    
    private BlacksmithController controller;
    private ObjectSwitch bench;
    private float wakeUpTime;
    private float offWorkTime; 
    private string homeToForgePathName;
    private string forgeToHomePathName;

    // Tasks
    private BlacksmithCompleteDailyRoutineTask completeDailyRoutineTask;
    private ForgeTask forgeTask;
    private SetPathTask homeToForgePathTask;
    private SetPathTask forgeToHomePathTask;
    private WorkTask workTask;
    private ObjectSwitchTask activateBenchTask;
    private ObjectSwitchTask deactivateBenchTask;

    public BlacksmithPlanner(BlacksmithController controller, WorldState worldstate, ObjectSwitch bench) {
        this.controller = controller;
        _worldState = worldstate;
        this.bench = bench;
    }

    public void SetPathNames(string homeToForgePathName, string forgeToHomePathName) {
        this.homeToForgePathName = homeToForgePathName;
        this.forgeToHomePathName = forgeToHomePathName;
    }

    public void SetTimes(float wakeUpTime, float offWorkTime) {
        this.wakeUpTime = wakeUpTime;
        this.offWorkTime = offWorkTime;
    }

    public override void SetupTaskTree() {

        _taskStack = new Stack<ITask>();

        //// SET UP TASKS
        // Low-level
        activateBenchTask = new ObjectSwitchTask(bench, true);
        deactivateBenchTask = new ObjectSwitchTask(bench, false);
        homeToForgePathTask = new SetPathTask(controller, homeToForgePathName);
        forgeToHomePathTask = new SetPathTask(controller, forgeToHomePathName);
        workTask = new WorkTask(controller, offWorkTime);
        // High-level
        forgeTask = new ForgeTask(homeToForgePathTask, forgeToHomePathTask, workTask, activateBenchTask, deactivateBenchTask);
        // Root
        completeDailyRoutineTask = new BlacksmithCompleteDailyRoutineTask(controller, wakeUpTime, forgeTask);

        // Set root node
        _rootNode = completeDailyRoutineTask;

        // Set up task tree
        _taskTree = new TaskTree(_rootNode);

    }

    // Add second task to child list of first task
    protected override void AddTaskToTree(ITask parentTask, ITask childTask) {

        if (parentTask is ICompositeTask compositeTask) {
            compositeTask.AddChild(childTask);
        }
    }

    // Execute current task and add children to 
    public override ITask Execute() {

        _worldState.UpdateWorldState();

        // Ensure that the stack is initialized
        if (_taskStack == null) {
            _taskStack = new Stack<ITask>();
        }

        // If the stack is empty, push the root node onto the stack
        if (_taskStack.Count == 0 && _rootNode != null) {
            _taskStack.Push(_rootNode);
        }

        // If there are tasks in the stack
        if (_taskStack.Count > 0) {
            ITask currentTask = _taskStack.Peek();

            // Execute the task if it's not already running
            if (currentTask.IsExecutable(_worldState) && !currentTask.IsRunning()) {
                Debug.Log(currentTask);
                currentTask.Execute(_worldState);
            }
            else if (!currentTask.IsRunning()) {
                _taskStack.Pop();

                if (_taskStack.Count > 0) {
                    currentTask = _taskStack.Peek();
                }
            }

            // If the task is complete, move to the next one
            if (currentTask.IsComplete(_worldState)) {
                _taskStack.Pop();

                // Add children of the completed task to the stack
                if (currentTask is ICompositeTask compositeTask) {
                    foreach (var child in compositeTask.GetChildren()) {
                        Debug.Log("Adding " + child);
                        _taskStack.Push(child);
                    }
                }
            }
        }

        // Return the current task (might be useful for debugging or further processing)
        if (_taskStack.Count > 0) {
            return _taskStack.Peek();
        }
        else {
            return null;
        }
    } 

}