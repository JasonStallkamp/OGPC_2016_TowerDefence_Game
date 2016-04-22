using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
	/// <summary>
	/// range of the tower
	/// </summary>
    [SerializeField]
    float TowerRange;

	/// <summary>
	/// squared range of the tower for preformance improvement
	/// </summary>
    private float TowerRangeSquared;

	/// <summary>
	/// damage of the tower
	/// </summary>
    [SerializeField]
    int damage;

	/// <summary>
	/// cost of the tower
	/// </summary>
    [SerializeField]
    int cost;

	/// <summary>
	/// name of the tower
	/// </summary>
    [SerializeField]
    string name;

	/// <summary>
	/// is the tower a sense tower
	/// </summary>
    [SerializeField]
    public bool isSenseTower;

    /// <summary>
    /// projectile that the tower fires
    /// </summary>
    [SerializeField]
    UnityEngine.GameObject Projectile;

	/// <summary>
	/// tower cool down length
	/// </summary>
    [SerializeField]
    float CoolDownLength;

	/// <summary>
	/// time of the last shot
	/// </summary>
    private float lastShot;
    
	/// <summary>
	/// The damage upgrade.
	/// </summary>
	[HideInInspector]
    public int damageUpgrade;

    // Use this for initialization
    void Start()
    {
		//calculate the tower range
        TowerRangeSquared = TowerRange * TowerRange;
		//set the damage upgrade to 0
        damageUpgrade = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//check if the tower is a sence tower
        if (isSenseTower == false)
        {
			//find the first enemy in range
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject target in enemies)
                if ((gameObject.transform.position - target.transform.position).sqrMagnitude < TowerRangeSquared)
                {
                    //check that the tower's cooldown has been complete
                    if (Time.time - lastShot > CoolDownLength)
                        if (enemies.Length > 0) //
                        {
                            //create a projectile
                            GameObject obj = (GameObject)Instantiate(Projectile, transform.position, this.transform.rotation);
                            ProjectileAI ObjProjectile = (obj.GetComponent("ProjectileAI") as ProjectileAI);
							//set the projectile's target and damage
                            ObjProjectile.Target = enemies[0];
                            ObjProjectile.damage = damage + damageUpgrade;
							//set the tower to cooldown
                            lastShot = Time.time;
                        }
                }
        }
        else
        {
			//find all towers in range
            GameObject[] Towers = GameObject.FindGameObjectsWithTag("Tower");
            foreach (GameObject Obj in Towers)
            {
                if ((gameObject.transform.position - Obj.transform.position).sqrMagnitude < TowerRangeSquared )
                {
					//give them a damage upgrade
                    Tower ToUpgrade = Obj.GetComponent<Tower>();
                    if(ToUpgrade.isSenseTower == false)
                        ToUpgrade.damageUpgrade += (int)Mathf.Round(ToUpgrade.damage * .25f);
                }
            }
        }
        damageUpgrade = 0; //  reset damage upgrade for towers

    }
    void OnDrawGizmos()
    {
		//draw range in editor
        List<Vector3> RadiusPositions = new List<Vector3>();
        for (int i = 0; i < 16; i++)
            RadiusPositions.Add(new Vector3(TowerRange * Mathf.Cos(i * (2 * Mathf.PI / 16)), 0, TowerRange * Mathf.Sin(i * (2 * Mathf.PI / 16))) + transform.position);

        Gizmos.color = Color.blue;
        for (int x = 0; x < RadiusPositions.Count - 1; x++)
            Gizmos.DrawLine(RadiusPositions[x], RadiusPositions[x + 1]);
        Gizmos.DrawLine(RadiusPositions[RadiusPositions.Count - 1], RadiusPositions[0]);
    }







    public int getCost()
    {
        return cost;
    }

    public string getName()
    {
        return name;
    }


}
