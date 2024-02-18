using UnityEngine;

[CreateAssetMenu(fileName = "NewPlaneData", menuName = "Plane Data", order = 1)]
public class PlaneData : ScriptableObject
{
    public string planeType;
    public string modelName;
    public string series;
}