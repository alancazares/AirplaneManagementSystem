using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Reference to the buttons
    public Button start;
    public Button generate;
    public Button liftoff;
    public Button lights;
    public Button land;

    void Start()
    {
        // Add click listeners to the buttons
        start.onClick.AddListener(OnClickButtonStart);
        generate.onClick.AddListener(OnClickButtonStart);
        liftoff.onClick.AddListener(OnClickButton1);
        lights.onClick.AddListener(OnClickButton2);
        land.onClick.AddListener(OnClickButton3);

    }

    void OnClickButtonStart()
    {
    }
    void OnClickButtonGenerate()
    {
    }

    // Button 1 click event handler
    void OnClickButton1()
    {
    }

    // Button 2 click event handler
    void OnClickButton2()
    {
    }

    // Button 3 click event handler
    void OnClickButton3()
    {
    }


    public void ToggleCommandButtons(bool but1, bool but2, bool but3)
    {
        liftoff.interactable = but1;
        lights.interactable = but2;
        land.interactable = but3;
    }
}