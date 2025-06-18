using System.Collections.Generic;
using UnityEngine;

// input manager singleton
public class InputManager {

    private Dictionary<string, KeyCode[]> keyMappings;

    private static InputManager instance;
    public static InputManager Instance {
        get {
            if (instance == null) {
                instance = new InputManager();
            }
            return instance;
        }
        private set { instance = value; }
    }

    public InputManager() {
        InitializeKeyMappings();
    }

    private void InitializeKeyMappings()
    {
        keyMappings = new Dictionary<string, KeyCode[]> {
            { Commands.MoveUp, new KeyCode[] { KeyCode.W, KeyCode.UpArrow } },
            { Commands.MoveDown, new KeyCode[] { KeyCode.S, KeyCode.DownArrow } },
            { Commands.MoveLeft, new KeyCode[] { KeyCode.A, KeyCode.LeftArrow } },
            { Commands.MoveRight, new KeyCode[] { KeyCode.D, KeyCode.RightArrow } },
            { Commands.Interact, new KeyCode[] { KeyCode.E, KeyCode.F } },
            { Commands.Inventory, new KeyCode[] { KeyCode.Tab, KeyCode.I, KeyCode.Q } },
            { Commands.Pause, new KeyCode[] { KeyCode.Escape } },
            { Commands.Roll, new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift } },
            { Commands.OpenMap, new KeyCode[] { KeyCode.Z } },
            { Commands.UseWeapon, new KeyCode[] { KeyCode.Mouse0 } },
            { Commands.Jump, new KeyCode[] { KeyCode.Space } },
        };
    }

    public bool IsKeyPressedOnce(string ability)
    {
        if (keyMappings.ContainsKey(ability)) {
            foreach (KeyCode key in keyMappings[ability]) {
                if (Input.GetKeyDown(key)) {
                    return true;
                }
            }
        }
        return false;
    }


    // check if key is pressed
    public bool IsKeyPressed(string ability)
    {
        if (keyMappings.ContainsKey(ability)) {
            foreach (KeyCode key in keyMappings[ability]) {
                if (Input.GetKey(key)) {
                    return true;
                }
            }
        }
        return false;
    }

    // bind action to different key
    public void RebindKey(string ability, KeyCode[] newKey)
    {
        if (keyMappings.ContainsKey(ability)) {
            keyMappings[ability] = newKey;
        }
    }
}
