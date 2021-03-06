using UnityEngine;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour
{
    public static Waypoints instance;
    private List<Transform[]> wayPoints = new List<Transform[]>();
    private int currentPath = -1;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < transform.childCount; i++) // Foreach path, add the path to the list
        {
            Transform[] path = new Transform[transform.GetChild(i).childCount];
		    for (int j = 0; j < path.Length; j++)
		    {
                path[j] = transform.GetChild(i).GetChild(j);;
		    }
            wayPoints.Add(path);
		}
    }

    public List<Transform[]> GetWaypoints()
    {
        return wayPoints;
    }

    public int GiveNewPath() // Gives out the path
    {
        currentPath++;
        if (currentPath >= wayPoints.Count)
        {
            currentPath = 0;
        }
        return currentPath;
    }

    public int GivePath() // Gives out the path
    {
        return currentPath;
    }
}
