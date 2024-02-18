using UnityEngine;

public class LightToggleEvent : MonoBehaviour
{
    // Define a delegate type for the light toggle event
    public delegate void LightToggleAction(bool state);

    // Define an event for the light toggle
    public static event LightToggleAction OnLightToggle;

    // Method to toggle the light
    public static void ToggleLight(bool state)
    {
        // Trigger the light toggle event
        OnLightToggle?.Invoke(state);
    }
}
