using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class WorldState {

    public enum WeatherType {
        Sunny,
        Rainy,
        Snowy,
        Cloudy
    }

    public float Time = ClockManager.Instance.InGameTime;
    public ClockManager.DayOfWeek Day = ClockManager.Instance.CurrentDay;
    
    public WeatherManager.WeatherStates Weather;
    //public NPCInventory Inventory;

    public WorldState() {
        //Inventory = new NPCInventory();
    }

    public void UpdateWorldState() {
        Time = ClockManager.Instance.InGameTime;
        Day = ClockManager.Instance.CurrentDay;
        Weather = WeatherManager.WeatherStates.Sunny;
    }

}