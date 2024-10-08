using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemManager : MonoBehaviour {

    public EcosystemGraph ecosystemGraph;

    private void Awake() {
        ecosystemGraph = new EcosystemGraph();
    }

    public void AddToGraph(EcosystemEntity entity) {
        if (ecosystemGraph != null) {
            ecosystemGraph.AddEntity(entity);
        }
    }
}