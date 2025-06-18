// primitive tasks that do not decompose
public interface ITask {
    
    bool CanExecute(WorldState worldState);
    void Execute(WorldState worldState);
    bool IsComplete();
    bool IsRunning();
}
