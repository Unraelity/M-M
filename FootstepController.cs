using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("Bindings")]
    [Tooltip("The Audio Source that will be used for footsteps")]
    [SerializeField] private AudioSource source;
    [Header("Volume")]
    [Tooltip("The volume the foosteps will play at.")]
    [SerializeField] private float volume = 0.3f;

    private void Awake()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
            }
        }
    }

    public void PlayFootstep()
    {
        Vector2Int currTileCoordinates = AStarGrid.Instance.MapToCoordinates(transform.position);
        Tile currTile = AStarGrid.Instance.Grid[currTileCoordinates.x, currTileCoordinates.y];
        AudioManager.Instance.PlayClip(source, currTile.FootstepSoundKey, volume);
    }
}
