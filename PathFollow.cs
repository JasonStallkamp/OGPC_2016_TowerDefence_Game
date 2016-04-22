using UnityEngine;
using System.Collections;


public class PathFollow : MonoBehaviour
{
	/// <summary>
	/// how fast the enemy moves
	/// </summary>
    [SerializeField]
    float movementScalor = .05f;

	//offset for the model
    [SerializeField]
    Vector3 offset;

	/// <summary>
	/// health of the enemy
	/// </summary>
    [SerializeField]
    public int health = 100;

	/// <summary>
	/// hidden max health for the enemy
	/// </summary>
    [HideInInspector]
    public int maxHealth;

	/// <summary>
	/// does the enemy rotate around origin
	/// </summary>
    [SerializeField]
    bool doesRotate;

	/// <summary>
	/// how accurate does the enemy follow the path
	/// </summary>
    const float errorbar = .05f;

	/// <summary>
	/// which point is the enemy currently going to
	/// </summary>
    int CurrentPoint;

	/// <summary>
	/// the amount of money the enemy gives
	/// </summary>
    public int money;

	/// <summary>
	/// the current rotation
	/// </summary>
    private Quaternion moveTo;


	public delegate void Manager(GameObject obj);
	/// <summary>
	///  event for when the object dies
	/// </summary>
	public Manager WaveManager;
	/// <summary>
	/// distance that the enemy has traveled
	/// </summary>
    public float traveledDistance;


    // Use this for initialization
    void Start()
    {
		//create the path if it has not been created
        if (PathManager.isInit == false)
            PathManager.start();
		// start at the path location
        gameObject.transform.position = PathManager.PathPoints[0] + offset;
		//set health and max health as the same
        maxHealth = health;
		// give the object a random rotation
        moveTo = Random.rotation;
		// set traveled distance to 0
        traveledDistance = 0;

    }

    // Update is called once per frame
    void Update()
    {
		//check that the enemy has not finished the path
        if (CurrentPoint != PathManager.PathPoints.Count)
        {
			//check if the enemy has reached the next point
            Vector3 ObjectPosition = gameObject.transform.position - offset;
            if (Mathf.Abs(ObjectPosition.x - PathManager.PathPoints[CurrentPoint].x) < errorbar &&
                Mathf.Abs(ObjectPosition.y - PathManager.PathPoints[CurrentPoint].y) < errorbar &&
                Mathf.Abs(ObjectPosition.z - PathManager.PathPoints[CurrentPoint].z) < errorbar)
                CurrentPoint++; // increment the point if the enemy has reached the last one
            if (CurrentPoint != PathManager.PathPoints.Count)
            {
				//find the direction of the next point
                Vector3 movement = (PathManager.PathPoints[CurrentPoint] - (ObjectPosition));
                movement.Normalize();
				//set the move distance to the next point
                movement = movement * movementScalor;
                traveledDistance += movement.magnitude;
				//move towards the next point
                gameObject.transform.Translate(movement);
                
            }
			//check that the enemy has not died
            if (health <= 0)
            {
				// if dead add money to the player
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerMovement>().money += money;
				// add the score to the path
                PathManager.Score += Mathf.Round(maxHealth * (1 - (traveledDistance / PathManager.pathLength)));
                //delete self
				Destroy(gameObject);
				// tell the path that the enemy is dead
				WaveManager.Invoke(this.gameObject);
            }
			//check that the enemy rotates
            if (doesRotate)
            {
				//rotate if the enemy rotates
                transform.GetChild(0).localRotation = Quaternion.RotateTowards(transform.GetChild(0).localRotation, moveTo, 30f);
                moveTo = Quaternion.RotateTowards(moveTo , Random.rotation, 45f);
            }

        }
        else
        {
			//if the enemy has reached the end remove one health to the end object
            GameObject.FindGameObjectWithTag("ProtectedObject").GetComponent<ProtectedObject>().health -= 1;
			//delete self
            Destroy(gameObject);
			//tell the path that the enemy has been deleted
			WaveManager.Invoke (this.gameObject);
        }
    }

    void OnGUI()
    {
		//check that the object has not been deleted
        if (gameObject != null)
        {
			// find the current 3 dimensional position on the screen
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position + Vector3.up);// gets screen position.\
            screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
            Rect rect = new Rect(screenPosition.x - 50,
            screenPosition.y - 12, 100, 24);// makes a rect centered at the player ( 100x24 )

			//create a texture to draw a health bar
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.red);
            texture.Apply();
            GUIStyle healthBarStyle = new GUIStyle();
            healthBarStyle.normal.background = texture;

			//check that the health is more then -1
            if (health >= 0)
            {
				// draw the health bar
                Rect healthBar = new Rect(rect.x + 4, rect.y + 4, (rect.width - 8) * ((float)(health) / (float)(maxHealth)), 16);
                GUI.Box(healthBar, GUIContent.none, healthBarStyle);
                GUI.Box(rect, System.Convert.ToString(health), GUI.skin.button);
            }
            else
            {
				//draw the health bar as 0
                GUI.Box(rect, System.Convert.ToString(0), GUI.skin.button);
            }

        }

        

    }
}