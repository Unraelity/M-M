using System;
using UnityEngine;

public enum GameDayOfWeek
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

public class ClockManager : MonoBehaviour
{
    public const int totalDayTime = 1440;  // time in a full day (set to 24 minutes) (every hour is 60 seconds)
    public const int totalHours = 24;

    public static ClockManager Instance { get; private set; }
    public static int secondsInHour;

    [Header("In Game Times and Day of Week")]
    [SerializeField] private string timeString = "12:00 AM";
    [SerializeField] private float currTime;   // current time 
    [SerializeField] private GameDayOfWeek dayOfWeek;
    [Header("In Game Time Scale")]
    [SerializeField] private float timeScale = 1f;

    public event Action<GameDayOfWeek> OnDayChanged;
    public float CurrTime => currTime;
    public float TotalDayTime => totalDayTime;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        secondsInHour = totalDayTime / totalHours;

        //currTime = 0f;    // start time at 12AM
        //currTime = 241;    // start time at 4:01AM
        currTime = 721;    // start time at 12:01PM

        dayOfWeek = GameDayOfWeek.Sunday;
    }

    private void Update()
    {
        // if game is paused return

        currTime += Time.deltaTime * timeScale;

        if (currTime >= totalDayTime)
        {
            NextDayOfWeek();
            OnDayChanged.Invoke(dayOfWeek);
            currTime = 0f;
        }

        timeString = GetTimeString(currTime);
    }

    private void NextDayOfWeek()
    {
        if (dayOfWeek == GameDayOfWeek.Saturday)
        {
            dayOfWeek = GameDayOfWeek.Sunday;
        }
        else
        {
            dayOfWeek++;
        }
    }

    // convert time float to readable string
    private static string GetTimeString(float time)
    {
        int hour = Mathf.FloorToInt(time / secondsInHour);  // get hour from division
        int minute = Mathf.FloorToInt(time % secondsInHour);  // get minutes from remainder

        string convention;

        // if it is less than noon set to AM
        if (time < (totalDayTime / 2))
        {
            convention = "AM";

            // if it is 12 am set hour to 12
            if (hour == 0)
            {
                hour = 12;
            }
        }
        // or else set to PM
        else
        {

            convention = "PM";
            if (hour != 12)
            {
                hour -= 12;     // if it is past noon subtract by 2 to get correct time
            }
        }

        string minuteString;

        // make formatting for minutes look nicer for single digit minutes
        if (minute < 10)
        {
            minuteString = "0" + minute.ToString();
        }
        else
        {
            minuteString = minute.ToString();
        }

        string timeString = hour + ":" + minuteString + " " + convention;
        return timeString;
    }

    // if minTime is greater than maxTime the check is different
    public static bool IsInInterval(float min, float max, bool invertedInterval = false)
    {

        float currTime = Instance.CurrTime;

        if (!invertedInterval)
        {

            if ((min < currTime) && (max > currTime))
            {
                return true;
            }
        }
        else
        {

            if ((min > currTime) && (max < currTime))
            {
                return true;
            }
        }

        return false;
    }
}
