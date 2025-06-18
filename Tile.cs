using UnityEngine;

public class Tile
{
    private Vector2 position;
    private bool isBlocked;
    private LayerMask targetLayer;
    private string footstepSoundKey;
    private float friction;
    
    public Vector2 Position => position;
    public bool IsBlocked => isBlocked;
    public string FootstepSoundKey => footstepSoundKey;
    public float Friction => friction;

    public Tile(Vector2 position)
    {
        Initialize(position);
    }

    public Tile(Vector2 position, TileData tileData)
    {
        Initialize(position);
        footstepSoundKey = tileData.FootstepSoundKey;
        friction = tileData.Friction;
    }

    private void Initialize(Vector2 position)
    {
        this.position = position;
        string layerName = "Grounded";

        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            targetLayer |= 1 << layer;
        }

        UpdateTile();
    }

    public void UpdateTile()
    {
        Vector2 overLap = new Vector2(AStarGrid.Instance.TileSize, AStarGrid.Instance.TileSize);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, overLap, 0f);

        foreach (Collider2D collider in colliders)
        {

            if ((!collider.isTrigger) && ((targetLayer & (1 << collider.gameObject.layer)) != 0))
            {

                isBlocked = true;
                return;
            }
        }

        isBlocked = false;
    }

    public void UpdateTile(GameObject ignoreObject)
    {
        Vector2 overLap = new Vector2(AStarGrid.Instance.TileSize, AStarGrid.Instance.TileSize);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, overLap, 0f);

        foreach (Collider2D collider in colliders)
        {

            if (collider.transform.root == ignoreObject.transform.root)
            {
                continue;
            }

            if ((!collider.isTrigger) && ((targetLayer & (1 << collider.gameObject.layer)) != 0))
            {

                isBlocked = true;
                return;
            }
        }

        isBlocked = false;
    }
}