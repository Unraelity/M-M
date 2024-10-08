using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowData {
    public ShadowController Controller;
    public Transform Transform;
}


public class DayNightCycle : MonoBehaviour {

    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private Light2D sun; // Reference to the moon Spot Light2D component
    [SerializeField] private Light2D moon; // Reference to the sun Spot Light2D component
    [SerializeField] private Light2D globalLight; // Reference to the Global Light2D component
    [SerializeField] private float xStart = 10f; // X position where the sun starts (far east)
    [SerializeField] private float xEnd = -10f; // X position where the sun sets (far west)
    [SerializeField] private Gradient sunColorGradient; // Gradient for sun color change
    [SerializeField] GameObject[] shadows = new GameObject[0];

    private ClockManager clockManager;

    private List<ShadowData> shadowDataList = new List<ShadowData>();

    private float dayStart = 241f;
    private float dayEnd = 1200f;
    private float nightStart = 1201f;
    private float nightEnd = 240f;
    private float cycleLength; // Total length of a day in seconds (24 minutes)

    private void Awake() {

        clockManager = ClockManager.Instance;
    }

    private void Start() {

        EnableShadows();
        cycleLength = clockManager.FullDayLength;

        foreach (GameObject shadow in shadows) {
            var shadowController = shadow.GetComponent<ShadowController>();
            var shadowTransform = shadow.transform;
            shadowDataList.Add(new ShadowData { Controller = shadowController, Transform = shadowTransform });
        }
    }

    private void Update() {
        float time = clockManager.InGameTime;

        float posX;

        if (IsDay(time)) {

            float timePercent;
            GetLightPos(time, dayStart, dayEnd, out posX, out timePercent);

            // Day cycle
            sun.enabled = true;
            moon.enabled = false;

            // Set sun position to follow the player's Y coordinate
            sun.transform.position = new Vector3(posX, player.transform.position.y, sun.transform.position.z);

            // Calculate light intensity based on position
            float midPoint = 0.5f;
            float intensity;

            if (timePercent <= midPoint) {
                intensity = Mathf.Lerp(0, 1, timePercent / midPoint);
            }
            else {
                intensity = Mathf.Lerp(1, 0, (timePercent - midPoint) / midPoint);
            }
            globalLight.intensity = intensity;

            // Update sun light color
            float gradientPercent = timePercent < 0.5f ? timePercent * 2 : (1 - timePercent) * 2;
            sun.color = sunColorGradient.Evaluate(gradientPercent);

            HandleShadows(timePercent);
        }
        else {

            // Night cycle
            sun.enabled = false;
            moon.enabled = true;

            // Adjust position for the moon similarly
            float nightTimePercent;
            GetLightPos(time, nightStart, nightEnd, out posX, out nightTimePercent);
            moon.transform.position = new Vector3(posX, player.transform.position.y, moon.transform.position.z);

            globalLight.intensity = 0;

            HandleShadows(nightTimePercent);
        }
    }

    // Determine from current time if it is day or night
    private bool IsDay(float time) {

        if (dayStart < dayEnd) {
            return time >= dayStart && time <= dayEnd;
        }
        else {
            return time >= dayStart || time <= dayEnd;
        }
    }

    private void GetLightPos(float time, float start, float end, out float posX, out float timePercent) {
        if (start < end) {
            // Regular cycle (e.g., day from 6 AM to 6 PM)
            timePercent = (time - start) / (end - start);
        } else {
            // Wrap-around cycle (e.g., night from 6 PM to 6 AM)
            float adjustedTime;

            if (time >= start) {
                // Time is after the start of the cycle (e.g., after 6 PM)
                adjustedTime = time - start;
            } else {
                // Time is before the end of the cycle (e.g., before 6 AM)
                adjustedTime = time + (cycleLength - start);
            }

            // Calculate the total length of the wrap-around period
            float cycleLengthAdjusted = cycleLength - start + end;
            timePercent = adjustedTime / cycleLengthAdjusted;
        }

        // Ensure timePercent is clamped between 0 and 1 to avoid overflow or underflow issues
        timePercent = Mathf.Clamp01(timePercent);

        // Calculate the position based on the time percent
        posX = Mathf.Lerp(xStart, xEnd, timePercent);
    }

    private void EnableShadows() {

        foreach (GameObject shadow in shadows) {

            shadow.SetActive(true);
        }
    }

    public void DisableShadows() {

        foreach (GameObject shadow in shadows) {

            shadow.SetActive(false);
        }
    }

    private void HandleShadows(float timePercent) {

        foreach (ShadowData shadowData in shadowDataList) {

            if (shadowData.Controller != null && !shadowData.Controller.FixedShadow) {

                // Calculate the rotation angle based on time of day
                float rotationAngle = Mathf.Lerp(shadowData.Controller.ShadowStartingAngle, shadowData.Controller.ShadowEndingAngle, timePercent);

                // Calculate the scale based on time of day
                float shadowScaleY;
                if (timePercent <= 0.5f) {
                    shadowScaleY = Mathf.Lerp(shadowData.Controller.MaxShadowLength, shadowData.Controller.MinShadowLength, timePercent * 2);
                } 
                else {
                    shadowScaleY = Mathf.Lerp(shadowData.Controller.MinShadowLength, shadowData.Controller.MaxShadowLength, (timePercent - 0.5f) * 2);
            }

                shadowData.Transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAngle));
                shadowData.Transform.localScale = new Vector3(shadowData.Transform.localScale.x, shadowScaleY, shadowData.Transform.localScale.z);
            }
        }
    }
}