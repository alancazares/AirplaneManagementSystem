using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private Color[] colors = { Color.gray, Color.yellow, Color.green }; // Array of predefined colors

    private Renderer rend; // Reference to the renderer component

    private int currentColorIndex = 0; // Index of the current color

    void Start()
    {
        rend = GetComponent<Renderer>(); // Get the renderer component attached to this GameObject
        if (rend == null)
        {
            Debug.LogError("Renderer component not found!");
        }

        // Apply the initial color
        ApplyColor();
    }

    // Method to apply the current color to the renderer
    void ApplyColor()
    {
        if (rend != null)
        {
            rend.material.color = colors[currentColorIndex];
        }
    }

    // Method to change to different color
    public void ChangeToSelectedColor(int value)
    {
        currentColorIndex = value;
        ApplyColor();
    }
}