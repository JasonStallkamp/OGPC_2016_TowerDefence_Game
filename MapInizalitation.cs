using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class MapInizalitation : MonoBehaviour
{



    // Use this for initialization
    void Start()
    {
        //UnityEngine.Object originalPlane = GameObject.Find("plane (0,0)");
        //Instantiate(originalPlane, new Vector3(1,1,0), new Quaternion(0, 0, 0, 0));
   		//create the path manager if it is not created
        if (PathManager.isInit == false)
            PathManager.start();


    }

    // Update is called once per frame
    void Update()
    {
		//update the path manager
        PathManager.update();
    }
}


/// <summary>
/// manager to deal with the handling of the path
/// </summary>
public static class PathManager
{
	//points that make up the ath
    public static List<Vector3> PathPoints;

	//has the path been created
    public static bool isInit = false;
	//length of the path
    public static float pathLength;
	//score for the path
    public static float Score;

	/// <summary>
	/// create the path
	/// </summary>
    public static void start()
    {
		Score = 0;
        isInit = true;
		// init the path
        PathPoints = new List<Vector3>();
        GameObject obj; // create a placeholder object
        int pointNumber = 0; // number of path points
        do
        {
			// grab the path a pathpoint
            obj = GameObject.Find("PathPoint (" + pointNumber + ")");
            if (obj != null) // check if one was found
                PathPoints.Add(obj.transform.position + Vector3.up); // add the point to the list
            pointNumber++; // increment the number of the path points
        }
        while (obj != null); // repeate while there is another path point

		//get the path length by finding the distance between each point
        for (int i = 1; i < PathPoints.Count; i++) 
            pathLength += (PathPoints[i] - PathPoints[i - 1]).magnitude;


		//make all of the pathpoints invisable
            GameObject[] toInvise = GameObject.FindGameObjectsWithTag("LogicObject");
		foreach (GameObject gameObje in toInvise)
			(gameObje.GetComponent ("MeshRenderer") as MeshRenderer).enabled = false;
    }
    public static void update()
    {
		//draw all points for the path
        for (int x = 0; x < PathPoints.Count - 1; x++)
            Debug.DrawLine(PathPoints[x], PathPoints[x + 1], Color.green);
    }

}