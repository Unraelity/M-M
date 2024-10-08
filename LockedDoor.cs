using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    public class LockedDoor : MonoBehaviour
    {
        public enum DoorStates { Open, Closed }
        public enum LockStates { Unlocked, Locked }

        [Header("Bindings")]
        [Tooltip("The object to show when the door is closed")]
        public GameObject ClosedDoor;

        [Header("State")]
        [Tooltip("The current state of the door")]
        public DoorStates DoorState = DoorStates.Open;
        [Tooltip("The current lock state of the door")]
        public LockStates LockState = LockStates.Unlocked;

        [MMInspectorButton("ToggleDoor")]
        public bool ToggleDoorButton;
        [MMInspectorButton("OpenDoor")]
        public bool OpenDoorButton;
        [MMInspectorButton("CloseDoor")]
        public bool CloseDoorButton;
        [MMInspectorButton("LockDoor")]
        public bool LockDoorButton;
        [MMInspectorButton("UnlockDoor")]
        public bool UnlockDoorButton;
        [MMInspectorButton("ToggleLock")]
        public bool ToggleLockButton;

        protected virtual void Start()
        {
            if (DoorState == DoorStates.Open)
            {
                SetDoorOpen();
            }
            else
            {
                SetDoorClosed();
            }
        }

        public virtual void OpenDoor()
        {
            if (LockState == LockStates.Unlocked && DoorState == DoorStates.Closed)
            {
                DoorState = DoorStates.Open;
            }
        }

        public virtual void CloseDoor()
        {
            if (DoorState == DoorStates.Open)
            {
                DoorState = DoorStates.Closed;
            }
        }

        public virtual void ToggleDoor()
        {
            if (LockState == LockStates.Unlocked)
            {
                if (DoorState == DoorStates.Open)
                {
                    DoorState = DoorStates.Closed;
                }
                else
                {
                    DoorState = DoorStates.Open;
                }
            }
        }

        public virtual void LockDoor()
        {
            if (DoorState == DoorStates.Closed)
            {
                LockState = LockStates.Locked;
            }
        }

        public virtual void UnlockDoor()
        {
            LockState = LockStates.Unlocked;
        }

        public virtual void ToggleLock()
        {
            if (DoorState == DoorStates.Closed)
            {
                if (LockState == LockStates.Unlocked)
                {
                    LockDoor();
                }
                else
                {
                    UnlockDoor();
                }
            }
        }

        protected virtual void Update()
        {
            if (ClosedDoor == null)
            {
                return;
            }

            if (DoorState == DoorStates.Open)
            {
                if (ClosedDoor.activeInHierarchy)
                {
                    SetDoorOpen();
                }
            }
            else
            {
                if (!ClosedDoor.activeInHierarchy)
                {
                    SetDoorClosed();
                }
            }
        }

        protected virtual void SetDoorClosed()
        {
            ClosedDoor.SetActive(true);
        }

        protected virtual void SetDoorOpen()
        {
            ClosedDoor.SetActive(false);
        }
    }
}