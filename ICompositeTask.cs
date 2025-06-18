using System.Collections.Generic;

// tasks that decompose
public interface ICompositeTask : ITask {

    // return children (decompose)
    List<ITask> Decompose(WorldState worldState);
}
