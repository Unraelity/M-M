using System.Collections.Generic;
using UnityEngine;

// detects obstacles inside of collider within indicated layers 
public class ObstacleDetection : MonoBehaviour 
{
    [SerializeField] private string[] layerNames = { "Grounded" };
    private LayerMask targetLayers;
    private HashSet<GameObject> overlapping = new HashSet<GameObject>();
    public bool HasTargetsInRange => overlapping.Count > 0;

    private void Awake() {

        foreach (string name in layerNames)
        {
            int layer = LayerMask.NameToLayer(name);
            if (layer != -1) {
                targetLayers |= 1 << layer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == gameObject) {
            return;
        }

        if (IsInTargetLayer(other.gameObject.layer))
        {
            overlapping.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

        if (other.gameObject == gameObject)
        {
            return;
        }

        if (IsInTargetLayer(other.gameObject.layer))
        {
                overlapping.Remove(other.gameObject);
        }
    }

    private bool IsInTargetLayer(int layer)
    {
        return (targetLayers & (1 << layer)) != 0;
    }
}
