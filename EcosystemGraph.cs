using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemGraph {

    private List<EcosystemEntity> entities;
    private Dictionary<string, EcosystemEntity> entityLookup;

    public EcosystemGraph() {

        entities = new List<EcosystemEntity>();
        entityLookup = new Dictionary<string, EcosystemEntity>();
    }

    public void AddEntity(EcosystemEntity entity) {

        if (!entityLookup.ContainsKey(entity.Type.Name))
        {
            entities.Add(entity);
            entityLookup[entity.Type.Name] = entity;
        }
    }

    public EcosystemEntity GetEntityByName(string name) {

        entityLookup.TryGetValue(name, out EcosystemEntity entity);
        return entity;
    }

    public void AddRelationship(string predatorName, string preyName) {

        EcosystemEntity predator = GetEntityByName(predatorName);
        EcosystemEntity prey = GetEntityByName(preyName);

        if (predator != null && prey != null) {
            predator.AddPrey(prey);
        }
    }
}
