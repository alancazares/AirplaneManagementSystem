using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{
    public Image redSquare; // Reference to the red square UI image
    private GameObject currentSelection; // Reference to the currently selected plane


    public Text[] PanelTexts;


    PlaneScript selectedPlaneScript;
    public PlaneData planeInformation;


    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Shoot a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object with a Plane tag
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Plane"))
                {

                    // If a plane is clicked, select it
                    SelectPlane(hit.collider.gameObject);

                    //Get the object's Plane Script and Info card
                    GameObject hitObject = hit.collider.gameObject;
                    selectedPlaneScript = hitObject.GetComponent<PlaneScript>();
                    GetPlaneInfoCard();

                    //post the Plane Information
                    PostPlaneInformation();
                }
                else
                {
                    // If something other than a plane is clicked, deselect the current selection
                    DeselectCurrent();
                }
            }
            else
            {
                // If nothing is clicked, deselect the current selection
                DeselectCurrent();
            }
        }

        // If there is a current selection, update the position of the red square to match its position
        if (currentSelection != null && redSquare != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentSelection.transform.position);
            redSquare.rectTransform.position = screenPos;
            redSquare.gameObject.SetActive(true); // Ensure the red square is always active when there is a selection
        }
        else
        {
            // If there is no current selection, hide the red square
            if (redSquare != null)
            {
                redSquare.gameObject.SetActive(false);
            }
        }
    }

    void SelectPlane(GameObject plane)
    {
        // Deselect the current selection if there is one
        DeselectCurrent();

        // Set the new selection
        currentSelection = plane;
    }

    void DeselectCurrent()
    {
        // If there is a current selection, deselect it
        currentSelection = null;
        ClearInformation();
    }

    void GetPlaneInfoCard()
    {
        planeInformation = selectedPlaneScript.ReturnInfoCard();
    }

    void PostPlaneInformation()
    {

        Debug.Log("should write info");
        if (planeInformation != null)
        {
            //Type
            PanelTexts[0].text = "Type: " + planeInformation.planeType;
            //Model
            PanelTexts[1].text = "Model: " + planeInformation.modelName;
            //Series
            PanelTexts[2].text = "Series: " + planeInformation.series;
        }
    }

    void ClearInformation()
    {
        //Type
        PanelTexts[0].text = "Type: Select a Plane";
        //Model
        PanelTexts[1].text = "Model: Select a Plane";
        //Series
        PanelTexts[2].text = "Series: Select a Plane";
    }
}
