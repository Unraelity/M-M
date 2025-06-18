using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{

    [Header("Day and Night Times")]
    [SerializeField] private float dayStart = 241f;
    [SerializeField] private float dayEnd = 1200f;
    [SerializeField] private float nightStart = 1201f;
    [SerializeField] private float nightEnd = 240f;

    [Header("Lighting Settings")]
    [SerializeField] private Light2D globalLight; // reference to the Global Light2D component
    [SerializeField] private float minLight = 0.2f;
    [SerializeField] private float maxLight = 1f;

    [Header("Dawn and Dusk Times")]
    [SerializeField] private float dawnStart = 240; // dawn start at 4:00 AM
    [SerializeField] private float dawnEnd = 285; // dawn ends at 4:45 AM
    [SerializeField] private float duskStart = 1140f; // dusk starts at 7:00 PM
    [SerializeField] private float duskEnd = 1200f; // dusk ends at 8:00 PM

    [Header("Sky Gradients")]
    [SerializeField] private Gradient dawnGradient; // gradient for sunrise color change
    [SerializeField] private Gradient duskGradient; // gradient for sunset color change

    private ClockManager clockManager;
    private float cycleLength; // total length of a day in seconds (24 minutes)
    private float currMaxLight = 1f;

    public static DayNightCycle Instance { get; private set; }
    public event Action<float> OnShadowUpdate;

    private void Awake()
    {
        if ((Instance != null) && (Instance != this))
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (globalLight == null)
        {
            globalLight = GetComponent<Light2D>();

            if (globalLight == null)
            {
                Debug.LogError("No GlobalLight2D assigned to DayNightCycle!");
            }
        }
    }

    private void Start()
    {
        clockManager = ClockManager.Instance;
        cycleLength = clockManager.TotalDayTime;
        currMaxLight = maxLight;
    }

    private void Update()
    {
        float time = clockManager.CurrTime;

        if (IsDay(time))
        {
            HandleDayTime(time);
        }
        else
        {
            // night cycle
            float timePercent = GetTimePercent(time, nightStart, nightEnd);

            globalLight.intensity = minLight;

            OnShadowUpdate?.Invoke(timePercent);
        }
    }
    
    public void ChangeMaxLight()
    {
        currMaxLight = maxLight; 
    }

    public void ChangeMaxLight(float deltaMaxLight)
    {
        currMaxLight = maxLight + deltaMaxLight;
    }

    // determine from current time if it is day
    private bool IsDay(float time)
    {
        return time >= dayStart && time <= dayEnd;
    }

    // determine from current time if it is dawn
    private bool IsDawn(float time)
    {
        return time >= dawnStart && time <= dawnEnd;
    }

    // determine from current time if it is dusk
    private bool IsDusk(float time)
    {
        return time >= duskStart && time <= duskEnd;
    }

    private void HandleDayTime(float time)
    {
        // day cycle
        float timePercent = GetTimePercent(time, dayStart, dayEnd);

        // calculate light intensity based on position
        float midPoint = 0.5f;
        float intensity;

        if (timePercent <= midPoint)
        {
            intensity = Mathf.Lerp(minLight, currMaxLight, timePercent / midPoint);
        }
        else
        {
            intensity = Mathf.Lerp(currMaxLight, minLight, (timePercent - midPoint) / midPoint);
        }
        globalLight.intensity = intensity;

        // update sky color based on time of day
        // sunrise color
        if (IsDawn(time))
        {
            UpdateColor(time, dawnStart, dawnEnd, dawnGradient);
        }
        // sunset color
        else if (IsDusk(time))
        {
            UpdateColor(time, duskStart, duskEnd, duskGradient);
        }

        OnShadowUpdate?.Invoke(timePercent);
    }

    private float GetTimePercent(float time, float start, float end)
    {
        float timePercent;

        if (start < end)
        {
            // regular cycle (e.g., day from 6 AM to 6 PM)
            timePercent = (time - start) / (end - start);
        }
        else
        {
            // wrap-around cycle (e.g., night from 6 PM to 6 AM)
            float adjustedTime;

            if (time >= start)
            {
                // time is after the start of the cycle (e.g., after 6 PM)
                adjustedTime = time - start;
            }
            else
            {
                // time is before the end of the cycle (e.g., before 6 AM)
                adjustedTime = time + (cycleLength - start);
            }

            // calculate the total length of the wrap-around period
            float cycleLengthAdjusted = cycleLength - start + end;
            timePercent = adjustedTime / cycleLengthAdjusted;
        }

        // ensure timePercent is clamped between 0 and 1 to avoid overflow or underflow issues
        timePercent = Mathf.Clamp01(timePercent);
        return timePercent;
    }

    private void UpdateColor(float time, float start, float end, Gradient gradient)
    {
        float halfPoint = (start + end) / 2f;
        float gradientPercent;

        // if time current time is less than half, move from end of gradient to beginning
        if (time < halfPoint)
        {
            gradientPercent = 1f - (time - start) / (halfPoint - start);
        }
       // if time current time is greater than half, move from beginning of gradient to end
        else
        {
            gradientPercent = (time - halfPoint) / (end - halfPoint);
        }

        globalLight.color = gradient.Evaluate(Mathf.Clamp01(gradientPercent));
    }
}