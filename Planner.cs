using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Planner {
    
    protected TaskTree _taskTree;
    protected WorldState _worldState;
    protected ITask _rootNode;
    protected Stack<ITask> _taskStack; 

    public abstract void SetupTaskTree();
    public abstract ITask Execute();
    protected abstract void AddTaskToTree(ITask parentTask, ITask childTask);
}