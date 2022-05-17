using UnityEngine;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour
{
    public static List<Transform[]> wayPoints = new List<Transform[]>();
    public static int currentPath = -1;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform[] path = new Transform[transform.GetChild(i).childCount];
		    for (int j = 0; j < path.Length; j++)
		    {
                path[j] = transform.GetChild(i).GetChild(j);
		    }
            wayPoints.Add(path);
		}
    }

    public static int GivePath()
    {
        currentPath++;
        if (currentPath >= wayPoints.Count)
        {
            currentPath = 0;
        }
        return currentPath;
    }
}
