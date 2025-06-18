using UnityEngine;


public class Door : MonoBehaviour, IInteractable
{
    public enum DoorStates { Open, Closed }
    public enum LockStates { Unlocked, Locked }

    [Header("Bindings")]
    [Tooltip("The object to show when the door is closed.")]
    [SerializeField] private GameObject closedDoor;
    [Tooltip("The Audio Source that will be used for opening and closing the door")]
    [SerializeField] private AudioSource source;

    [Header("Audio Settings")]
    [Tooltip("The volume the door will play at when closed or opened.")]
    [SerializeField] private float volume = 0.5f;
    [Tooltip("Key string for when door opens.")]
    [SerializeField] private string doorOpenAudioKey = AudioLabels.Door_Wooden_Open;
    [Tooltip("Key string for when door closes.")]
    [SerializeField] private string doorCloseAudioKey = AudioLabels.Door_Wooden_Close;
    
    // door states
    private DoorStates doorState = DoorStates.Closed;
    private LockStates lockState = LockStates.Unlocked;

    public DoorStates DoorState => doorState;
    public LockStates LockState => lockState;

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

    public void Interact()
    {
        ToggleDoor();
    }

    public void OpenDoor()
    {
        if (doorState == DoorStates.Closed)
        {
            if (lockState == LockStates.Unlocked)
            {
                doorState = DoorStates.Open;
                AudioManager.Instance.PlayClip(source, doorOpenAudioKey, volume);
                if ((closedDoor != null) && closedDoor.activeInHierarchy)
                {
                    closedDoor.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Door is locked!");
            }
        }
    }

    public void CloseDoor()
    {
        if (doorState == DoorStates.Open)
        {
            doorState = DoorStates.Closed;
            AudioManager.Instance.PlayClip(source, doorCloseAudioKey, volume);
            if ((closedDoor != null) && (!closedDoor.activeInHierarchy))
            {
                closedDoor.SetActive(true);
            }
        }
    }

    public void ToggleDoor()
    {
        switch (doorState)
        {
            case DoorStates.Open:
                CloseDoor();
                break;
            case DoorStates.Closed:
            default:
                OpenDoor();
                break;
        }
    }

    public void LockDoor()
    {
        if (doorState == DoorStates.Closed)
        {
            lockState = LockStates.Locked;
        }
    }

    public void UnlockDoor()
    {
        lockState = LockStates.Unlocked;
    }
}