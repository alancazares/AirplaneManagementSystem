using UnityEngine;

public class LightController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Subscribe to the light toggle event
        LightToggleEvent.OnLightToggle += ToggleLight;
    }

    void OnDestroy()
    {
        // Unsubscribe from the light toggle event
        LightToggleEvent.OnLightToggle -= ToggleLight;
    }

    void ToggleLight(bool state)
    {
        // Toggle the sprite renderer based on the received state
        spriteRenderer.enabled = state;
    }
}

