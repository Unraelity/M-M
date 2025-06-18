using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private string levelKey = "Luminae";

    [SerializeField] private string[] layerNames = { "Grounded" };
    private LayerMask targetLayers;

    private void Awake() {

        foreach (string name in layerNames) {

            int layer = LayerMask.NameToLayer(name);
            if (layer != -1) {
                targetLayers |= 1 << layer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsInTargetLayer(other.gameObject.layer))
        {
            LevelManager.Instance.LoadScene(levelKey);
        }
    }

    private bool IsInTargetLayer(int layer) {
        return (targetLayers & (1 << layer)) != 0;
    }
}
