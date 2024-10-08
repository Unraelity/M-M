using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class WeatherManager : MonoBehaviour {

    public enum WeatherStates { Sunny, Rainy, Stormy, Snowy, Cloudy }

    [Header("Weather Settings")]
    public WeatherStates CurrentWeather = WeatherStates.Sunny;

    [Header("Buttons")]
    [MMInspectorButton("SetSunny")]
    public bool SetSunnyButton;
    [MMInspectorButton("SetRainy")]
    public bool SetRainyButton;
    [MMInspectorButton("SetStormy")]
    public bool SetStormyButton;
    [MMInspectorButton("SetSnowy")]
    public bool SetSnowyButton;
    [MMInspectorButton("SetCloudy")]
    public bool SetCloudyButton;

    [Header("Particle Systems")]
    public ParticleSystem RainParticleSystem;
    public ParticleSystem StormParticleSystem;
    public ParticleSystem SnowParticleSystem;

    private void Start() {
        ApplyWeather(CurrentWeather);
    }

    public void SetSunny() {
        ChangeWeather(WeatherStates.Sunny);
    }

    public void SetRainy() {
        ChangeWeather(WeatherStates.Rainy);
    }

    public void SetStormy() {
        ChangeWeather(WeatherStates.Stormy);
    }

    public void SetSnowy() {
        ChangeWeather(WeatherStates.Snowy);
    }

    public void SetCloudy() {
        ChangeWeather(WeatherStates.Cloudy);
    }

    private void ChangeWeather(WeatherStates newWeather) {
        CurrentWeather = newWeather;
        ApplyWeather(CurrentWeather);
    }

    // Logic to update the environment based on weather
    private void ApplyWeather(WeatherStates weather) {
        // Deactivate all particle systems first
        DeactivateAllParticles();

        switch (weather) {
            case WeatherStates.Sunny:
                // No particle system for sunny weather
                break;
            case WeatherStates.Rainy:
                if (RainParticleSystem != null) RainParticleSystem.Play();
                break;
            case WeatherStates.Stormy:
                if (StormParticleSystem != null) StormParticleSystem.Play();
                break;
            case WeatherStates.Snowy:
                if (SnowParticleSystem != null) SnowParticleSystem.Play();
                break;
            case WeatherStates.Cloudy:
                // Add effects for cloudy weather if you want
                break;
        }
    }

    private void DeactivateAllParticles() {
        if (RainParticleSystem != null) RainParticleSystem.Stop();
        if (StormParticleSystem != null) StormParticleSystem.Stop();
        if (SnowParticleSystem != null) SnowParticleSystem.Stop();
    }
}