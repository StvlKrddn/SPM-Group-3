using UnityEngine;
using System.Collections.Generic;

public class Waypoints : MonoBehaviour
{
    public static List<Transform[]> wayPoints = new List<Transform[]>();
    public static int currentPath = 0;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int c = 0;
            Transform[] path = new Transform[transform.GetChild(i).childCount];
		    for (int j = 0; j < path.Length; j++)
		    {
                c++;
                path[j] = transform.GetChild(i).GetChild(j);
		    }
            print(c);
            wayPoints.Add(path);
		}
    }

    public static int GivePath()
    {
        currentPath++;
        if (currentPath > wayPoints.Count -1)
        {
            currentPath = 0;
        }
        return currentPath;
    }
}
