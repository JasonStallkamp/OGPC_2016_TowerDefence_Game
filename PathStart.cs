using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathStart : MonoBehaviour
{
	/// <summary>
	/// Data For Each Wave
	/// </summary>
    [System.Serializable]
    public class WaveData
    {
        public GameObject Unit;
        public int NumberOfUnits;
        public float spawnRate;
		public float RestTime;
        public int OverrideHealth = 0;
    }

	/// <summary>
	/// List of Waves
	/// </summary>
    [SerializeField]
    List<WaveData> enemyList;
	public List<GameObject> LiveEnemys = new List<GameObject>();

	//Last time a enemy was spawned
    float lastSpawn;
    public int Wave {get { return wave; }}
    public int Enemy { get { return enemy; } }
	/// <summary>
	/// the Wave Nubmer
	/// </summary>
    int wave;
	/// <summary>
	/// the enemy number
	/// </summary>
    int enemy;

	//Is the Game ready to go to the next wave
    [HideInInspector]
	public bool CanNextWave;

	/// <summary>
	/// Time since the last wave
	/// </summary>
    [HideInInspector]
    public float LastWave;

	// string form of time to next wave
    [HideInInspector]
    public string TimeToNextWave;

	// is this the final wave
    [HideInInspector]
    public bool isFinalWave = false;

    // Use this for initialization
    void Start()
    {
		//set wave and enemy to 0
        wave = 0;
        enemy = 0;
       //set the current wave timer to 0
        TimeToNextWave = "";
    }

    // Update is called once per frame
    void Update()
    {
		//check if there is more waves
        if(wave < enemyList.Count && Time.time - lastSpawn > enemyList[wave].spawnRate)
        {
			//check if the wave has more enemys
            if (enemyList[wave].NumberOfUnits > enemy)
            {
				// set the time for last wave
				LastWave = 0;
				// increment enemy
                enemy++;

				//create a new enemy
            lastSpawn = Time.time;
				GameObject NewEnemy = (GameObject)Instantiate (enemyList [wave].Unit, PathManager.PathPoints [0], Quaternion.identity);
				NewEnemy.GetComponent<PathFollow> ().WaveManager = new PathFollow.Manager(RemoveEnemy);
				LiveEnemys.Add(NewEnemy);
                NewEnemy.tag = "Enemy";
				//override the health of the enemy if requested
                if (enemyList[wave].OverrideHealth != 0)
                {
                    NewEnemy.GetComponent<PathFollow>().maxHealth = enemyList[wave].OverrideHealth;
                    NewEnemy.GetComponent<PathFollow>().health = enemyList[wave].OverrideHealth;
                }
				// set the time to next wave to blank
                TimeToNextWave = "";
            }
			else
			{
				//check if there are no enemys alive 
				if (LiveEnemys.Count < 1) {
					//set can next wave
					CanNextWave = true;
					if (LastWave == 0) {
						LastWave = Time.time;

					}
					// decide if this is the final wave
                    if (enemyList[wave].RestTime == -1)
                    {
                        CanNextWave = true;
                        isFinalWave = true;  
                    }
					// decide if the next wave is set on a timer
                    else if (enemyList [wave].RestTime != 0) {
						if (Time.time - LastWave > enemyList [wave].RestTime) {
							nextWave ();
						}
						TimeToNextWave = " - " + System.Convert.ToString (Mathf.Round (enemyList [wave].RestTime - (Time.time - LastWave)));

					}
				}
			}
        }
    }

	/// <summary>
	/// remove an enemy from the live enemy list
	/// </summary>
	/// <param name="enemy">Enemy to be removed</param>
	public void RemoveEnemy(GameObject enemy)
	{
		LiveEnemys.Remove(enemy);
	}

	/// <summary>
	/// increment the wave
	/// </summary>
    public void nextWave()
    {
		//increment the wave,
        wave++;
		//reset the enemy number
        enemy = 0;
		//tell the game that it cant go to the next wave yet
		CanNextWave = false;
    }
}
