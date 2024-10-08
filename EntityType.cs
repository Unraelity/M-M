using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityType {
    public string Name { get; private set; }
    public bool IsSelfPredatory { get; private set; }

    public EntityType(string name, bool isSelfPredatory = false) {
        Name = name;
        IsSelfPredatory = isSelfPredatory;
    }
}