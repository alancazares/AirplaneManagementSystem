using UnityEngine;
using UnityEngine.UI;

public class SliderTextUpdater : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public float initialValue = 3f; // Initial value for the slider

    void Start()
    {
        // Set the initial value of the slider
        slider.value = initialValue;

        // Update the text with the initial value
        UpdateText(initialValue);

        // Subscribe to the slider's OnValueChanged event
        slider.onValueChanged.AddListener(UpdateText);
    }

    void UpdateText(float value)
    {
        // Convert the float value to an integer
        int intValue = Mathf.RoundToInt(value);

        // Update the text component with the integer value
        if (text != null)
        {
            text.text = intValue.ToString();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the slider's OnValueChanged event when the script is destroyed
        slider.onValueChanged.RemoveListener(UpdateText);
    }
}