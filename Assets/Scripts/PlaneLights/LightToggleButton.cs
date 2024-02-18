using UnityEngine;
using UnityEngine.UI;

public class LightToggleButton : MonoBehaviour
{
    // Reference to the button's text component (optional)
    public Text buttonText;

    private bool isLightOn = false; // Track the state of the light

    void Start()
    {
        // Set the initial text of the button (optional)
        UpdateButtonText();
    }

    public void ToggleLight()
    {
        // Toggle the state of the light
        isLightOn = !isLightOn;

        // Raise the light toggle event
        LightToggleEvent.ToggleLight(isLightOn);

        // Update the text of the button (optional)
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        // Update the text of the button based on the current state of the light
        if (buttonText != null)
        {
            buttonText.text = isLightOn ? "Turn Off Lights" : "Turn On Lights";
        }
    }
}


