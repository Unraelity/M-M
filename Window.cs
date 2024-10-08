using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    public class Window : MonoBehaviour
    {
        public enum WindowStates { Open, Closed }

        [Header("Bindings")]
        [Tooltip("The object to show when the window is open")]
        public GameObject OpenedWindow;

        [Header("State")]
        [Tooltip("The current state of the window")]
        public WindowStates WindowState = WindowStates.Closed;

        [MMInspectorButton("ToggleWindow")]
        public bool ToggleWindowButton;
        [MMInspectorButton("OpenWindow")]
        public bool OpenWindowButton;
        [MMInspectorButton("CloseWindow")]
        public bool CloseWindowButton;

        /// <summary>
        /// On Start, we initialize the window based on its initial status
        /// </summary>
        protected virtual void Start()
        {
            if (WindowState == WindowStates.Open)
            {
                SetWindowOpen();
            }
            else
            {
                SetWindowClosed();
            }
        }

        /// <summary>
        /// Opens the window
        /// </summary>
        public virtual void OpenWindow()
        {
            WindowState = WindowStates.Open;
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        public virtual void CloseWindow()
        {
            WindowState = WindowStates.Closed;
        }

        /// <summary>
        /// Toggles the window open or closed based on its current state
        /// </summary>
        public virtual void ToggleWindow()
        {
            if (WindowState == WindowStates.Open)
            {
                WindowState = WindowStates.Closed;
            }
            else
            {
                WindowState = WindowStates.Open;
            }
        }

        /// <summary>
        /// On Update, we open or close the window if needed
        /// </summary>
        protected virtual void Update()
        {
            if (OpenedWindow == null)
            {
                return;
            }

            if (WindowState == WindowStates.Open)
            {
                if (!OpenedWindow.activeInHierarchy)
                {
                    SetWindowOpen();
                }
            }
            else
            {
                if (OpenedWindow.activeInHierarchy)
                {
                    SetWindowClosed();
                }
            }
        }

        /// <summary>
        /// Opens the window
        /// </summary>
        protected virtual void SetWindowOpen()
        {
            OpenedWindow.SetActive(true);
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        protected virtual void SetWindowClosed()
        {
            OpenedWindow.SetActive(false);
        }
    }
}
