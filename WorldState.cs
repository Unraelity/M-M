using System.Collections.Generic;


// script that be passed through to HTN and GOAP keep track of state of the world
public class WorldState {
    
    public Dictionary<string, object> state = new();

    public void Set(string key, object value) => state[key] = value;
    public T Get<T>(string key) => (T)state[key];
}
