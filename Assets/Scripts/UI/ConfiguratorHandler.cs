using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguratorHandler : MonoBehaviour
{
    //reference to the UI elements
    public Button generate;
    public Slider hangarSlider;
    public Slider planeSlider;

    //set values
    int hangars = 0;
    int planes = 0;

    //Reference to the Control Tower Script
    public ControlTower controlTower;


    void TakeValues()
    {
        hangars = (int)hangarSlider.value;
        planes = (int)planeSlider.value;
    }

    public void GenerateConfiguration()
    {
        TakeValues();
        //pass values to control tower to generate all assets
        controlTower.SetHangarsAndPlanes(hangars, planes);
    }
        

}
