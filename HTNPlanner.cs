using System.Collections.Generic;

public abstract class HTNPlanner {

    private Stack<ITask> taskStack;

    public HTNPlanner() {
        taskStack = new Stack<ITask>();
    }

    protected abstract void Initialize();

    public ITask GetTask() {

        // if task stack is empty reinitialize
        if (taskStack.Count <= 0) {
            Initialize();
        }
        
        return taskStack.Pop();
    }

    public void AddToTaskStack(ITask task) {
        taskStack.Push(task);
    }

    public void AddListToTaskStack(List<ITask> taskList) {

        foreach (ITask task in taskList) {
            //Debug.Log("Adding: " + task + " to stack");
            taskStack.Push(task);
        }
    }
}
