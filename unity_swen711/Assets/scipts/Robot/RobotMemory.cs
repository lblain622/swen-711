using System.Collections.Generic;
using UnityEngine;

public class RobotMemory : MonoBehaviour
{
    private List<Vector3> dangerSpots = new List<Vector3>();

    public void RememberDanger(Vector3 position)
    {
        dangerSpots.Add(position);
    }

    public bool IsDangerous(Vector3 position)
    {
        foreach (var spot in dangerSpots)
        {
            if (Vector3.Distance(spot, position) < 2f)
                return true;
        }
        return false;
    }
}