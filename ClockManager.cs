using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClockManager : MonoBehaviour {

        public enum DayOfWeek {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    // Singleton instance
    private static ClockManager instance;
    [SerializeField] private string CurrentTime = "6:00";
    [SerializeField] private DayOfWeek currentDay = DayOfWeek.Sunday; // Using the enum for the current day

    public static ClockManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<ClockManager>();
                if (instance == null) {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<ClockManager>();
                    singleton.name = typeof(ClockManager).ToString();
                }
            }
            return instance;
        }
    }

    // Private backing field for in-game time
    private float inGameTime = 360f;
    
    // Public property with private set and public get
    public float InGameTime
    {
        get { return inGameTime; }
        private set { inGameTime = value; }
    }

    public DayOfWeek CurrentDay {
        get { return currentDay; }
        private set { currentDay = value; }
    }

    public float TimeSpeed = 1f; // Speed at which in-game time passes

    private const float fullDayLength = 1440;

    public float FullDayLength {
        get { return fullDayLength; }
        private set {}
    }

    private void Awake() {
        // Ensure that the instance is not destroyed between scenes
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Update() {
        // Only increment time if the game is not paused or frozen
        if (Time.timeScale > 0) {
            UpdateTime();
            PrintTime();
        }
    }

    private void UpdateTime() {
        if (inGameTime < fullDayLength) {
            inGameTime += Time.deltaTime * TimeSpeed;
        }
        else {
            inGameTime = 0f;
            AdvanceDay(); // Advance the day when a full day has passed
        }
    }

    private void AdvanceDay() {
        // Increment the current day
        currentDay++;

        // Wrap around if necessary
        if (currentDay > DayOfWeek.Sunday) {
            currentDay = DayOfWeek.Monday;
        }
    }

    private void PrintTime() {
        int totalMinutes = Mathf.FloorToInt(inGameTime);
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        CurrentTime = $"{hours:D2}:{minutes:D2} ({currentDay})"; // Include the day in the printed time
    }
}