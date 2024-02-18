using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    //Flight Routes
    public List<GameObject> wayPoints_hangarExit;
    public List<GameObject> wayPoints_path_takeOff;
    public List<GameObject> wayPoints_path_flight;
    public List<GameObject> wayPoints_path_landing;
    public List<GameObject> wayPoints_path_taxi;
    public List<GameObject> wayPoints_hangarEnter;
    public Vector3 inside;
    public Vector3 outside;

    List<GameObject> selectedPath;
    int pathIndex = 0;
    int waypointIndex = 0;

    //Plane Properties
    [SerializeField] float speed;
    [SerializeField] float fixedSpeed;
    [SerializeField] float speedMultiplier;
    Vector3 destination;
    float distance;
    public int assignedHangar = 0;

    //Will the plane remain in air?
    [SerializeField] bool flyLoop = true;

    //Control Tower Reference
    ControlTower controlTowerReference;

    //Plane Types
    public List<PlaneData> PlaneDataTypes;
    public PlaneData PlaneInfoCard;//Contains this Plane information


    void Start()
    {
        GenerateInfoCard();
        GetPlanePaths();
        CreateInsideOutside();
        SwitchPath();
        ComunicateToControlTower();
    }

    private void Update()
    {
        FollowPath();
        CheckDistance();
        RoutePlane(); //GameLogic
    }

    void GenerateInfoCard()
    {
        if (PlaneDataTypes.Count == 0)
        {
            Debug.LogError("PlaneData list is empty!");
            return;
        }

        // Get a random index within the range of the list
        int randomIndex = Random.Range(0, PlaneDataTypes.Count);

        // Get the PlaneData asset at the random index
        PlaneInfoCard = PlaneDataTypes[randomIndex];
    }

    void GetPlanePaths()
    {
        GameObject parentObject = GameObject.Find("Paths");

        if (parentObject != null)
        {
            foreach (Transform child in parentObject.transform)
            {
                // Get all child objects of each path and add them to the respective list
                List<GameObject> pathList = new List<GameObject>();

                foreach (Transform waypoint in child)
                {
                    pathList.Add(waypoint.gameObject);
                }

                // Check if the child name contains the desired keywords
                if (child.name.ToLower().Contains("takeoff"))
                {
                    wayPoints_path_takeOff.AddRange(pathList);
                }
                else if (child.name.ToLower().Contains("flight"))
                {
                    wayPoints_path_flight.AddRange(pathList);
                }
                else if (child.name.ToLower().Contains("landing"))
                {
                    wayPoints_path_landing.AddRange(pathList);
                }
                else if (child.name.ToLower().Contains("taxi"))
                {
                    wayPoints_path_taxi.AddRange(pathList);
                }
                else
                {
                    Debug.LogWarning("Unknown path type: " + child.name);
                }
            }
        }
        else
        {
            Debug.LogError("Parent object not found!");
        }
    }

    void CreateInsideOutside()
    {
        inside = transform.position;
        Vector3 newPosition = new Vector3(inside.x, inside.y, inside.z - 1);
        outside = newPosition;

        //Instantiate guide prefabs
        GameObject insideObject = new GameObject("Inside");
        GameObject outsideObject = new GameObject("Outside");
        insideObject.transform.position = inside;
        outsideObject.transform.position = outside;

        //add Objects as waypoints inside to outside
        wayPoints_hangarExit.Add(insideObject);
        wayPoints_hangarExit.Add(outsideObject);
        destination = outside;//initialized as the first destination

        //add Objects as waypoints outside to inside
        wayPoints_hangarEnter.Add(outsideObject);
        wayPoints_hangarEnter.Add(insideObject);

    }

    void SwitchPath()
    {
        //reset waypoint index counter
        waypointIndex = 0;

        if (pathIndex == 0)
        {
            selectedPath = wayPoints_hangarExit;
            speedMultiplier = .5f;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 1)
        {
            selectedPath = wayPoints_path_takeOff;
            speedMultiplier = .5f;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 2)
        {
            selectedPath = wayPoints_path_flight;
            speedMultiplier = 2;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 3)
        {
            selectedPath = wayPoints_path_landing;
            speedMultiplier = 1;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 4)
        {
            selectedPath = wayPoints_path_taxi;
            speedMultiplier = .5f;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 5)
        {
            selectedPath = wayPoints_hangarEnter;
            speedMultiplier = .5f;
            speed = fixedSpeed * speedMultiplier;
        }
        if (pathIndex == 6)
        {
            //End of the whole loop
            selectedPath = wayPoints_hangarExit;
            speed = 0f;

            //at the end of the trip plane reports back to base 
            ReportToControlTower();
        }
    }

    void FollowPath()
    {
        //Select destination on Selected Path
        destination = selectedPath[waypointIndex].transform.position;

        //Rotate towards new destination
        Vector3 targetDirection = destination - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1.0f, 1.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);

        //Move to new destination
        Vector3 newPos = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = newPos;
    }

    void CheckDistance()
    {
        //Check distance
        distance = Vector3.Distance(transform.position, destination);
    }

    void RoutePlane()
    {
        /**
         * if waypoint is reached
         * check if last waypoint
         * if not continue
         * else loop? or change path?
         **/

        //get close to the waypoint
        if (distance <= 0.05)
        {
            //Not last waypoint? continue
            if (waypointIndex < selectedPath.Count - 1)
            {
                waypointIndex++;
            }
            else
            {
                //Check if path index = 1
                //Loop in the air if not decided to land
                if (pathIndex == 2)
                {
                    if (flyLoop)
                    {
                        //Stay in the air
                        waypointIndex = 2;
                    }
                    else
                    {
                        //Change Path
                        pathIndex++;
                        SwitchPath();
                    }
                }
                else
                {
                    //Change Path
                    pathIndex++;
                    SwitchPath();
                }
            }
        }
    }

    public void EndFLightLoop()
    {
        flyLoop = false;
    }

    public void RestartFlight()
    {
        speedMultiplier = .5f;
        speed = fixedSpeed * speedMultiplier;
        waypointIndex = 0;
        flyLoop = true;
        pathIndex = 0;
        SwitchPath();
    }

    public void TakeHangarNumber(int value)
    {
        assignedHangar = value;
    }

    public int ReturnHangarNumber()
    {
        return assignedHangar;
    }
    private void ComunicateToControlTower()
    {
        GameObject foundObject = GameObject.Find("ControlTower");

        // Check if the GameObject was found
        if (foundObject != null)
        {
            //Debug.Log("Control Tower found");
            // Access the script component attached to the GameObject
            controlTowerReference = foundObject.GetComponent<ControlTower>();
        }
        else
        {
            Debug.LogWarning("GameObject not found with the name: NameOfGameObject");
        }
    }

    void ReportToControlTower()
    {
        // Check if the script component was found
        if (controlTowerReference != null)
        {
            // Now you can use the script component
            controlTowerReference.BackToBaseReport(assignedHangar);
        }
        else
        {
            Debug.LogWarning("Script component not found on GameObject: " + controlTowerReference.name);
        }
    }

     public PlaneData ReturnInfoCard()
    {
        return PlaneInfoCard;
    }

}