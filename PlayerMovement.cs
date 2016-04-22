using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class PlayerMovement : MonoBehaviour
{

    const float ANGLECONST = 0.7071f;
    Vector3 MousePositionMoving;
    [System.Serializable]
    public class TowerStorage
    {
        public Texture GuiTexture;
        public GameObject TowerObject;
        public string Name;
    }
    public int StartingMoney;

    [HideInInspector]
    public int money;
    [SerializeField]
    public List<TowerStorage> Towers;
    public GUISkin skin;
    private int towerPage;
    private int TowerPageMax;
    private int MouseLastX, MouseLastY;
    private bool music;
    private bool Options;
    private float buttonTime;
    public Texture NoTowerImage;
    public Texture MusicSymbol;
    public Texture GearSymbol;
    public Texture NoMusicSymbol;
    public bool justclicked;
    public Tower ThingToSell;

    [SerializeField]
    float cameraMouseSpeed = 1f;

    [SerializeField]
    float scrollSpeed = 1f;


    


    // Use this for initialization
    void Start()
    {
        //set page of towers to 0
        towerPage = 0;
        // set the maximum pages of towers
        TowerPageMax = Mathf.CeilToInt(((float)Towers.Count + 1) / 8) - 1;

        // get the mouse position
        MouseLastX = (int)Input.mousePosition.x;
        MouseLastY = (int)Input.mousePosition.y;
        //set the music to repeat
        music = PlayerPrefs.GetInt("music", 1) ==1;
        this.GetComponent<AudioListener>().enabled = music;

        //set starting money and initalize variables
        Options = false;
        money = StartingMoney;
        buttonTime = 0;
        justclicked = false;
        
    }

    public int selectorGrid;
    
    public void OnGUI()
    {
        #region UIRendering
        //load the gui skin
        GUI.skin = skin;
        int ResolutionWidth = Screen.width;
        int ResolutionHeight = Screen.height;
		PathStart WaveManager = GameObject.Find("PathPoint (0)").GetComponent<PathStart>();
        
        //draw the tower selection grid
        int boxNumber = ((Screen.height - 315) / 100) * 2;
        List<GUIContent> TowerSelectionGrid = new List<GUIContent>();
        for(int i = towerPage * boxNumber; i < Towers.Count && i < boxNumber + towerPage * boxNumber; i++)
            TowerSelectionGrid.Add(new GUIContent(Towers[i].Name + "\n<size=16>cost:" + Towers[i].TowerObject.GetComponent<Tower>().getCost() + "</size>", Towers[i].GuiTexture == null ? NoTowerImage : Towers[i].GuiTexture));
        for (int i = TowerSelectionGrid.Count; i < boxNumber; i++)
            TowerSelectionGrid.Add(new GUIContent(""));


        //draw the box for money and health;
        GUI.Box(new Rect(ResolutionWidth - 200, 0, 200, 100), new GUIContent("Health:" + GameObject.FindGameObjectWithTag("ProtectedObject").GetComponent<ProtectedObject>().health
            + "\n Money:" + money + "\n Score:" + PathManager.Score));
        selectorGrid = GUI.SelectionGrid(new Rect(ResolutionWidth - 200, 105, 200, ResolutionHeight - 315), selectorGrid, TowerSelectionGrid.ToArray(), 2);

        //draw the next button for tower page
        if (towerPage == 0 )
            GUI.Box(new Rect(ResolutionWidth - 98, ResolutionHeight - 205, 98, 100), new GUIContent("<color=#808080ff>next</color>"));
        else
            GUI.Button(new Rect(ResolutionWidth - 98, ResolutionHeight - 205, 98, 100), new GUIContent("next"));
        //draw the previous button for tower page
        if (towerPage == TowerPageMax)
            GUI.Box(new Rect(ResolutionWidth - 200, ResolutionHeight - 205, 98, 100), new GUIContent("<color=#808080ff>Previous</color>"));
        else
            GUI.Button(new Rect(ResolutionWidth - 200, ResolutionHeight - 205, 98, 100), new GUIContent("Previous"));

        //check if the player can go to the next wave
		if(WaveManager.CanNextWave)
		{
            //check if the level is finished
            if(WaveManager.isFinalWave)
            {
                // create finished texture   
                GUIStyle OuterBox = new GUIStyle( GUI.skin.box);
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                OuterBox.normal.background = texture;

                //display finished message and the button to end the game
                GUI.Label(new Rect(ResolutionWidth / 2 - 500, ResolutionHeight / 2 - 100, 500, 150), "",OuterBox);
                GUI.Label(new Rect(ResolutionWidth/2 - 500, ResolutionHeight/2 - 100, 500, 150), new GUIContent("<size=128><color=Black>Great Job</color></size>"));
                if (GUI.Button(new Rect(ResolutionWidth - 200, ResolutionHeight - 100, 200, 100), new GUIContent("Exit to Main Menu")))
                {
                    PlayerPrefs.SetInt("level:" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, (int)PathManager.Score);
                    
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
                
            }
            //otherwise show a button to go to next level
			else if( GUI.Button(new Rect(ResolutionWidth - 200, ResolutionHeight - 100, 200, 100), new GUIContent("Next Wave" + WaveManager.TimeToNextWave)))
			{
				WaveManager.nextWave();
			}
		}
		else
		{
			GUI.Box(new Rect(ResolutionWidth - 200, ResolutionHeight - 100, 200, 100), new GUIContent("Wave In Progress"));
		}

        //show options button
        if (GUI.Button(new Rect(8, ResolutionHeight - 40, 32, 32), GearSymbol))
            Options = !Options;

        //show music button
        if (GUI.Button(new Rect(48, ResolutionHeight - 40, 32, 32), music ? MusicSymbol : NoMusicSymbol))
        {
            music = !music;
            PlayerPrefs.SetInt("music", music ? 1 : 0);
            AudioListener.volume = music ? 1 : 0;
        }


        
        #endregion

        #region UIControls

        //move camera around
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetAxis("Mouse ScrollWheel") > 0 && Camera.main != null)
            Camera.main.orthographicSize = Mathf.Max(Mathf.Min(-8 * scrollSpeed * Input.GetAxis("Mouse ScrollWheel") + Camera.main.orthographicSize, 50), 5);

        //if user presses e open the options menu
        if (Input.GetKeyDown(KeyCode.Escape) & Time.unscaledTime - buttonTime > .5f)
        {
            Options = !Options;
            buttonTime = Time.unscaledTime;
        }

        //show options menu
        if (Options)
        {
            //draw options box
            Rect OptionsBox = new Rect((ResolutionWidth - 200) / 2, (ResolutionHeight - 132) / 2, 200, 132);
            GUI.Box(OptionsBox, "");

            //draw the quit buttons
            if (GUI.Button(new Rect(OptionsBox.xMin + (OptionsBox.width - 150) / 2, OptionsBox.yMin + 16, 150, 50), "Quit To Windows"))
                Application.Quit();
            if (GUI.Button(new Rect(OptionsBox.xMin + (OptionsBox.width - 150) / 2, OptionsBox.yMin + 66, 150, 50), "Quit To Main Menu"))
               UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        //show the diologue for selling towers
        else if (ThingToSell != null)
        {
            //convert 3d point to 2d screen position
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(ThingToSell.transform.position + Vector3.up);// gets screen position.\
            screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
            Rect rect = new Rect(screenPosition.x - 50,
            screenPosition.y - 12, 100, 60);// makes a rect centered at the player ( 100x24 )
            GUI.Box(rect, "", GUI.skin.box);
            //shjow sell button
            if(GUI.Button(new Rect(rect.xMin + 4,rect.yMin + 4,92,24), "Sell", GUI.skin.button))
            {
                money += ThingToSell.getCost() / 2;
                Destroy(ThingToSell.gameObject);
            }
            //show cancel button
            else if (GUI.Button(new Rect(rect.xMin + 4, rect.yMin + 28, 92, 24), "Cancel", GUI.skin.button))
            {
                ThingToSell = null;
            }
        }
        //check for user mouse click
        else if (Input.GetMouseButtonDown(0))
        {
            if (justclicked == false)
            {
                //check iuf the user clicked on a tile
                RaycastHit hit;
                Ray clicked3D = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(clicked3D, out hit, 100000f);
                if (hit.transform != null && hit.transform.gameObject.tag == "BuildTile")
                {
                    //if clicked on a tile create tower
                    BuildTile Tile = hit.transform.gameObject.GetComponent<BuildTile>();
                    if (Tile.Tower == null)
                    {
                        if (money >= Towers[selectorGrid].TowerObject.GetComponent<Tower>().getCost())
                        {
                            money -= Towers[selectorGrid].TowerObject.GetComponent<Tower>().getCost();
                            Tile.Tower = (Instantiate(Towers[selectorGrid].TowerObject, hit.transform.position + new Vector3(0, .75f, 0), new Quaternion()) as GameObject);
                        }
                    }
                }
                //if user clicked on tower show sell diologue
                else if (hit.transform != null && hit.transform.gameObject.tag == "Tower")
                {
                    ThingToSell = hit.transform.gameObject.GetComponent<Tower>();
                }
            }
            justclicked = !justclicked;
        }






        //move camera based on arrow keys and wasd
        MousePositionMoving = new Vector3(0, 0, 0);
        if(Input.GetMouseButton(2) == true)
        {
            MousePositionMoving = new Vector3(MouseLastX - Input.mousePosition.x , MouseLastY  - Input.mousePosition.y , 0) * (Camera.main.orthographicSize / 160);
        }
        MouseLastX = (int)Input.mousePosition.x;
        MouseLastY = (int)Input.mousePosition.y;
        Camera.main.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * cameraMouseSpeed, Input.GetAxis("Vertical") * cameraMouseSpeed, 0) + MousePositionMoving);


        //if user presses e turn the camera
        if(Input.GetKey(KeyCode.E) )
        {
            
            //Find point from camera and angle to y = zero point
            float angle;
            Vector3 axis;
            this.transform.rotation.ToAngleAxis(out angle, out axis);

            Vector3 LookDir = this.transform.rotation * new Vector3(0, 0, 1);
            Vector2 RatioDir = new Vector2(LookDir.x, LookDir.z);
            RatioDir.Normalize();



            //rotate around this point
            Vector3 endPoint = new Vector3(transform.position.x + RatioDir.x * transform.position.y, 0, transform.position.z + RatioDir.y * transform.position.y);
            this.transform.RotateAround(endPoint, Vector3.up, scrollSpeed / -3);
        }
        //if user presses e turn the camera
        if (Input.GetKey(KeyCode.Q))
        {
            //Find point from camera and angle to y = zero point
            float angle;
            Vector3 axis;
            this.transform.rotation.ToAngleAxis(out angle, out axis);

            Vector3 LookDir = this.transform.rotation * new Vector3(0, 0, 1);
            Vector2 RatioDir = new Vector2(LookDir.x, LookDir.z);
            RatioDir.Normalize();

            //rotate around this point
            Vector3 endPoint = new Vector3(transform.position.x + RatioDir.x * transform.position.y, 0, transform.position.z + RatioDir.y * transform.position.y);
            this.transform.RotateAround(endPoint, Vector3.up, scrollSpeed / 3);
        }
        #endregion
    }
}
