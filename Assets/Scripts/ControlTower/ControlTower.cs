using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTower : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints;
    List<Transform> activeSpawnPoint = new List<Transform>(); //keep track of activated spawn points
    public float spawnInterval = 1f; // Adjust this variable to set the interval between spawns

    //Hangars information
    [SerializeField] GameObject prefabHangar;
    public List<GameObject> hangars = new List<GameObject>();//List of all hangars
    public int ammountHangars;
    public int[] planesPerHangarCount;//tracks to which hangar the planes were assigned
    public int[] planesInHangarCount; //tracks the amount of planes back in base

    //all of the light controllers in the hangars
    public List<ChangeColor> allLightControllers = new List<ChangeColor>();
    public List<ChangeColor> activeHangarLights = new List<ChangeColor>();

    //all the hangar controllers
    public List<FenceController> allFenceControllers = new List<FenceController>();
    public List<FenceController> activeFenceControllers = new List<FenceController>();


    //Planes Information
    [SerializeField] GameObject prefabPlane;
    public int ammountPlanes;
    bool planesSpawned = false;
    bool flightRestarted = false;

    //Track all generated airplanes
    List<GameObject> allPlanes = new List<GameObject>();
    List<PlaneScript> allPlaneScripts = new List<PlaneScript>();

    public void SetHangarsAndPlanes(int hangars, int planes)
    {
        ammountHangars = hangars;
        ammountPlanes = planes;

        SpawnHangars();
        GetHangarsLightControllers();
        GetHangarsFenceControllers();
    }

    public void SpawnHangars()
    {
        //Initialize array to count assigned planes
        planesPerHangarCount = new int[ammountHangars];

        if (prefabHangar == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Object to spawn or spawn points are not set!");
            return;
        }
        // Limit the amount to spawn to the minimum of the list count and amountToSpawn
        int spawnCount = Mathf.Min(ammountHangars, spawnPoints.Count);


        for (int i=0; i< spawnCount; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject newHangar = Instantiate(prefabHangar, spawnPoint.position, spawnPoint.rotation);
            //Register the spawnpoint as activespawnpoint
            activeSpawnPoint.Add(spawnPoints[i]);

            //Add all hangars to a list
            hangars.Add(newHangar);
        }
    }

    public void GetHangarsLightControllers()
    {
        for (int i = 0; i < ammountHangars; i++)
        {
            //Get this hangar light and set active color
            Transform childLight = hangars[i].transform.Find("hangarLight");
            ChangeColor childChangeColor = childLight.GetComponent<ChangeColor>();
            allLightControllers.Add(childChangeColor);
        }
    }

    public void GetHangarsFenceControllers()
    {
        for (int i = 0; i < ammountHangars; i++)
        {
            //Get the hangar Fence Controller and open the door
            Transform childFence = hangars[i].transform.Find("FenceControl");
            FenceController fenceControl = childFence.GetComponent<FenceController>();
            allFenceControllers.Add(fenceControl);
        }
    }

    IEnumerator DeployPlanes()
    {
        if (prefabPlane == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Object to spawn or spawn points are not set!");
            yield break;
        }

        for (int i = 0; i < ammountPlanes; i++)
        {
            // Pick a random spawn point
            int randomHangarNumber = Random.Range(0, activeSpawnPoint.Count);
            Transform randomSpawnPoint = activeSpawnPoint[randomHangarNumber];

            // Spawn the object at the chosen spawn point
            GameObject NewPlane = Instantiate(prefabPlane, randomSpawnPoint.position, randomSpawnPoint.rotation);

            //assign the correspoding hangar number to plane
            PlaneScript thisPlane = NewPlane.GetComponent<PlaneScript>();
            thisPlane.TakeHangarNumber(randomHangarNumber);

            //Get this hangar light and set active color
            allLightControllers[randomHangarNumber].ChangeToSelectedColor(1);
            activeHangarLights.Add(allLightControllers[randomHangarNumber]);

            //Open the door
            allFenceControllers[randomHangarNumber].OpenFence();

            //Add to the PlaneLog and count
            allPlanes.Add(NewPlane);
            planesPerHangarCount[randomHangarNumber] +=1;

            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
        }

        GetAllPlaneScripts();
    }

    void GetAllPlaneScripts()
    {
        for (int i = 0; i < allPlanes.Count; i++)
        {
            // Get the PlaneScript for each plane in the loop
            PlaneScript planeScript = allPlanes[i].GetComponent<PlaneScript>();
            allPlaneScripts.Add(planeScript);
        }
    }

    public void CommandPlanesToLand()
    {
        for (int i = 0; i < allPlanes.Count; i++)
        {
            if (allPlaneScripts[i] != null)
            {
                // Access the public method in the PlaneScript
                allPlaneScripts[i].EndFLightLoop();
            }
            else
            {
                // Log a warning if no PlaneScript is found
                Debug.LogWarning("No PlaneScript found on GameObject: " + allPlanes[i].name);
            }
        }
    }

    public void CommandPlanesLiftoff()
    {

        //initialize array to count deployed planes
        planesInHangarCount = new int[ammountHangars];

        if (planesSpawned == false)
        {
            StartCoroutine(DeployPlanes());
            planesSpawned = true;
        }
        else
        {
            CommandPlanesRestartFlight();
        }
    }

    public void CommandPlanesRestartFlight()
    {
        if (flightRestarted == false)
        {
            StartCoroutine(RestartPlanesFlight());
            flightRestarted = true;
        }
        else
        {
            //hold off for planes to be in the air
            
        }
    }

    IEnumerator RestartPlanesFlight()
    {
        for (int i = 0; i < allPlanes.Count; i++)
        {
            if (allPlaneScripts[i] != null)
            {
                // Access the public method in the PlaneScript
                allPlaneScripts[i].RestartFlight();
                int hangarNumber = allPlaneScripts[i].ReturnHangarNumber();

                activeHangarLights[i].ChangeToSelectedColor(1);
                //activeFenceControllers[i].OpenFence();
                allFenceControllers[hangarNumber].OpenFence();
            }
            else
            {
                // Log a warning if no PlaneScript is found
                Debug.LogWarning("No PlaneScript found on GameObject: " + allPlanes[i].name);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        //once all planes are restarted we return this to false
        flightRestarted = false;
    }

    public void BackToBaseReport(int value)
    {
        planesInHangarCount[value] += 1;
        CheckHangarFull(value);
    }

    public void ResetHangarCount()
    {
        for (int i = 0; i < allPlanes.Count; i++)
        {
            planesInHangarCount[i] = 0;
        }
    }

    public void CheckHangarFull(int value)
    {
        if(planesPerHangarCount[value] == planesInHangarCount[value])
        {
            //Change to Full Color
            allLightControllers[value].ChangeToSelectedColor(2);

            //Close Fence
            allFenceControllers[value].CloseFence();
        }
    }
}
