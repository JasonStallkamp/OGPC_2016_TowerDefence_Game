using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MainMenuCamera : MonoBehaviour {


    public GUISkin skin;
    public Texture logo;
	public Texture brainMap;
    public Texture MusicSymbol;
    public Texture NoMusicSymbol;
    private bool music;
    private enum Menu { Main, LevelSelect, Info }
    Menu currentMenu;
    bool checkingReset;



	// Use this for initialization
	void Start () {
        currentMenu = Menu.Main;
        checkingReset = false;
        music = PlayerPrefs.GetInt("music", 1) == 1;
        AudioListener.volume = music ? 1 : 0;
        
    }
	
	// Update is called once per frame
	void Update () {

    }
    public void OnGUI()
    {

        GUIStyle buttonStyle = skin.button;
            

        if (GUI.Button(new Rect(16, Screen.height - 40, 32, 32), music ? MusicSymbol : NoMusicSymbol))
        {
            music = !music;
            AudioListener.volume = music ? 1 : 0;
            PlayerPrefs.SetInt("music", music ? 1 : 0);
            
        }
        Vector2 GUIStartLocations = new Vector2(Screen.width / 2, (Screen.height - 600) / 2);

        #region mainMenu
        if (currentMenu == Menu.Main)
        {

            if (checkingReset)
            {
                Rect OptionsBox = new Rect((Camera.current.pixelWidth - 200) / 2, (Camera.current.pixelHeight - 132) / 2, 200, 182);
                GUI.Box(OptionsBox, "");
                GUI.Label(new Rect(OptionsBox.left + (OptionsBox.width - 150) / 2, OptionsBox.top + 16, 150, 50), "Are You Sure? \n There's no going back");
                if (GUI.Button(new Rect(OptionsBox.left + (OptionsBox.width - 150) / 2, OptionsBox.top + 66, 150, 50), "No"))
                {
                    checkingReset = false;
                }
                if (GUI.Button(new Rect(OptionsBox.left + (OptionsBox.width - 150) / 2, OptionsBox.top + 116, 150, 50), "Yes I am sure"))
                {
                    PlayerPrefs.DeleteAll();
                    checkingReset = false;
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x - 384, GUIStartLocations.y + 50, 512 + 256, 200), logo, skin.box);
                if (GUI.Button(new Rect(GUIStartLocations.x - 256, GUIStartLocations.y + 250, 512, 100), "Play", buttonStyle))
                {
                    currentMenu = Menu.LevelSelect;
                }

                if (GUI.Button(new Rect(GUIStartLocations.x - 256, GUIStartLocations.y + 350, 512, 100), "Game Info", buttonStyle))
                {
                    currentMenu = Menu.Info;
                }

                if (GUI.Button(new Rect(GUIStartLocations.x - 256, GUIStartLocations.y + 450, 512, 100), "Exit", buttonStyle))
                {
                    Application.Quit();
                }
                if (GUI.Button(new Rect(Camera.current.pixelWidth - 256, Camera.current.pixelHeight - 100, 256, 100), "<size=48>reset</size>", buttonStyle))
                {
                    checkingReset = true;
                }

            }
        }
        #endregion


        #region settings
        if (currentMenu == Menu.Info)
        {
            GUI.Label(new Rect(GUIStartLocations.x - 384, GUIStartLocations.y -60, 512 + 256, 100), "<size=36>press any key to return</size>", skin.label);
            GUI.Box(new Rect(GUIStartLocations.x - 384, GUIStartLocations.y + 50, 512 + 256, 200), logo, skin.box);
            GUI.TextArea(new Rect(GUIStartLocations.x - 350, GUIStartLocations.y + 250, 700, 400),@"Move View - Arrow Keys
Selection - Mouse
Lead/Music                      Tyler French
Programmer                Jason Stallkamp
Artist                                 Krista Woo
3D Modeler                  Tyler Wakefield",skin.textArea);


            if (Input.anyKey == true)
            {
                currentMenu = Menu.Main;
               
            }
            
        }
        #endregion
        #region levelSelect
        if (currentMenu == Menu.LevelSelect)
        {
            GUI.Label(new Rect(GUIStartLocations.x - 256, GUIStartLocations.y, 512, 100),"Level Select",skin.label);
            GUI.DrawTexture(new Rect(GUIStartLocations.x - 450, GUIStartLocations.y + 50, 900, 600), brainMap);


            if (GUI.Button(new Rect(GUIStartLocations.x + 196, GUIStartLocations.y + 356, 96, 48), "Red" + "\n" + PlayerPrefs.GetInt("level:1",0)))
            {
                Application.LoadLevel(1);
            }

            #region Orange
            if (PlayerPrefs.GetInt("level:1", -1) != -1)
            {
                if (GUI.Button(new Rect(GUIStartLocations.x + 112, GUIStartLocations.y + 208, 96, 48), "Orange" + "\n" + PlayerPrefs.GetInt("level:2", 0)))
                {
                    Application.LoadLevel(2);
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x + 112, GUIStartLocations.y + 208, 96, 48), "Orange");
            }
            #endregion region
            #region Yellow
            if (PlayerPrefs.GetInt("level:2", -1) != -1)
            {
                if (GUI.Button(new Rect(GUIStartLocations.x - 36, GUIStartLocations.y + 332, 96, 48), "Yellow" + "\n" + PlayerPrefs.GetInt("level:3", 0)))
                {
                    Application.LoadLevel(3);
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x - 36, GUIStartLocations.y + 332, 96, 48), "Yellow");
            }
            #endregion region

            #region Green
            if (PlayerPrefs.GetInt("level:3", -1) != -1)
            {
                if (GUI.Button(new Rect(GUIStartLocations.x - 96, GUIStartLocations.y + 430, 96, 48), "Green" + "\n" + PlayerPrefs.GetInt("level:4", 0)))
                {
                    Application.LoadLevel(4);
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x - 96, GUIStartLocations.y + 430, 96, 48), "Green");
            }
            #endregion region

            #region Purple
            if (PlayerPrefs.GetInt("level:4", -1) != -1)
            {
                if (GUI.Button(new Rect(GUIStartLocations.x - 196, GUIStartLocations.y + 332, 96, 48), "Purple" + "\n" + PlayerPrefs.GetInt("level:5", 0)))
                {
                    Application.LoadLevel(5);
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x - 196, GUIStartLocations.y + 332, 96, 48), "Purple");
            }
            #endregion region

            #region Grey
            if (PlayerPrefs.GetInt("level:5", -1) != -1)
            {
                if (GUI.Button(new Rect(GUIStartLocations.x - 320, GUIStartLocations.y + 300, 96, 48), "Grey" + "\n" + PlayerPrefs.GetInt("level:6", 0)))
                {
                    Application.LoadLevel(6);
                }
            }
            else
            {
                GUI.Box(new Rect(GUIStartLocations.x - 320, GUIStartLocations.y + 300, 96, 48), "Grey");
            }
            #endregion region

            if (GUI.Button(new Rect(GUIStartLocations.x - 400, GUIStartLocations.y + 500, 256, 100), "back", buttonStyle))
            {
                currentMenu = Menu.Main;
            }
        }
        #endregion
    }
}


