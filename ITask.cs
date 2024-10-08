using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask {
    bool IsExecutable(WorldState worldState);
    void Execute(WorldState worldState);
    bool IsComplete(WorldState worldState);
    bool IsRunning();  // Add this method
}
