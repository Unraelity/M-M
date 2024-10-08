using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTree {

    private ITask rootTask;

    public TaskTree(ITask task) {
        rootTask = task;
    }

}