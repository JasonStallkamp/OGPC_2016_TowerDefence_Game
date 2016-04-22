using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//turn the object to always face the Camera
        transform.transform.rotation.Set(-Camera.current.transform.rotation.x, Camera.current.transform.rotation.y, -Camera.current.transform.rotation.z, Camera.current.transform.rotation.w);
    }
}
