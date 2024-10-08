using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemEntity {
    public EntityType Type { get; private set; }
    public List<EcosystemEntity> Predators { get; private set; }
    public List<EcosystemEntity> Prey { get; private set; }

    public EcosystemEntity(EntityType type) {
        Type = type;
        Predators = new List<EcosystemEntity>();
        Prey = new List<EcosystemEntity>();
    }

    public void AddPredator(EcosystemEntity predator) {

        if (!Predators.Contains(predator))
        {
            Predators.Add(predator);
            predator.AddPrey(this); // Add the reverse relationship
        }
    }

    public void AddPrey(EcosystemEntity prey) {

        if (!Prey.Contains(prey)) {
            Prey.Add(prey);
            prey.AddPredator(this); // Add the reverse relationship
        }
    }
}