using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompositeTask : ITask
{
    void AddChild(ITask childTask);
    void RemoveChild(ITask child);
    List<ITask> GetChildren();
}