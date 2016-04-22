using UnityEngine;
using System.Collections;

public class ProtectedObject : MonoBehaviour
{

	/// <summary>
	/// health of the protected object
	/// </summary>
    [SerializeField]
    public int health;
	/// <summary>
	/// max health of the object
	/// </summary>
    private int maxHealth;

    // Use this for initialization
    void Start()
    {
		//set the max health to the health
        maxHealth = health;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
		//check that the object is not destroyed
        if (gameObject != null && Camera.main != null)
        {
			//find the 3 dimensional object on the 2 dimensional screen
            Vector3 screenPosition =
    Camera.main.WorldToScreenPoint(gameObject.transform.position + Vector3.up);// gets screen position.\
            screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
            Rect rect = new Rect(screenPosition.x - 50,
            screenPosition.y - 12, 100, 24);

			// create a texture for the health bar
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.red);
            texture.Apply();
            GUIStyle healthBarStyle = new GUIStyle();
            healthBarStyle.normal.background = texture;

			// draw the health bar
            Rect healthBar = new Rect(rect.x + 4, rect.y + 4, (rect.width - 8) * ((float)(health) / (float)(maxHealth)), 16);
            GUI.Box(healthBar, GUIContent.none, healthBarStyle);
            GUI.Box(rect, System.Convert.ToString(health), GUI.skin.button);
        }
    }
}
