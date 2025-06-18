using UnityEngine;

public class Window : MonoBehaviour
{
    public enum WindowStates { Open, Closed }

    [Header("Bindings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("Sprite for when window is open.")]
    [SerializeField] private Sprite openWindowSprite;
    [Tooltip("Sprite for when window is closed.")]
    [SerializeField] private Sprite closedWindowSprite;
    // window states
    private WindowStates windowState = WindowStates.Closed;

    public WindowStates WindowState => windowState;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                if (closedWindowSprite != null)
                {
                    spriteRenderer.sprite = closedWindowSprite;
                }
            }
        }
    }

    public void OpenWindow()
    {
        if (windowState == WindowStates.Closed)
        {
            windowState = WindowStates.Open;
            if (openWindowSprite != null)
            {
                spriteRenderer.sprite = openWindowSprite;
            }
        }
    }

    public void CloseWindow()
    {
        if (windowState == WindowStates.Open)
        {
            windowState = WindowStates.Closed;
            if (closedWindowSprite != null)
            {
                spriteRenderer.sprite = closedWindowSprite;
            }
        }
    }
}
