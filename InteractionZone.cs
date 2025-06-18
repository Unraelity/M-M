using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    private IInteractable target;

    public IInteractable Target => target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            target = interactable;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if ((interactable != null) && (target == interactable)) {
            target = null;
        }
    }
}
