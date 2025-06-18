using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public enum WeatherType
    {
        Sunny,
        Cloudy,
        Rainy,
        Snowy,
        SnowStorm
    }
    
    [Header("Current Weather Type")]
    [SerializeField] private WeatherType currWeatherType = WeatherType.Sunny;
    [Header("Cloudy Lighting Changes")]
    [SerializeField] private float cloudyLightingChange = 0.2f;
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem rainEffect;
    [SerializeField] private ParticleSystem snowEffect;
    [SerializeField] private ParticleSystem snowStormEffect;
    private WeatherType lastSavedWeatherType;
    private DayNightCycle dayNightCycle;
    private ParticleSystem currentParticleSystem;

    public static WeatherManager Instance { get; private set; }
    public WeatherType CurrWeatherType
    {
        get { return currWeatherType; }
        private set { currWeatherType = value; }
    }

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
        dayNightCycle = DayNightCycle.Instance;
        lastSavedWeatherType = currWeatherType;
        ChangeWeather();
    }

    private void Update()
    {
        if (currWeatherType != lastSavedWeatherType)
        {
            ChangeWeather();
            lastSavedWeatherType = currWeatherType;
        }
    }

    public void ChangeWeather()
    {
        if (currentParticleSystem != null)
        {
            currentParticleSystem.Stop();
        }

        switch (currWeatherType)
        {
            case WeatherType.Sunny:
                dayNightCycle.ChangeMaxLight();
                currentParticleSystem = null;
                break;
            case WeatherType.Cloudy:
                dayNightCycle.ChangeMaxLight(-cloudyLightingChange);
                currentParticleSystem = null;
                break;
            case WeatherType.Rainy:
                dayNightCycle.ChangeMaxLight(-cloudyLightingChange);
                currentParticleSystem = rainEffect;
                currentParticleSystem.Play();
                break;
            case WeatherType.Snowy:
                dayNightCycle.ChangeMaxLight(-cloudyLightingChange);
                currentParticleSystem = snowEffect;
                currentParticleSystem.Play();
                break;
            case WeatherType.SnowStorm:
                dayNightCycle.ChangeMaxLight(-cloudyLightingChange);
                currentParticleSystem = snowStormEffect;
                currentParticleSystem.Play();
                break;
            default:
                dayNightCycle.ChangeMaxLight();
                currentParticleSystem = null;
                break;
        }
    }
}
