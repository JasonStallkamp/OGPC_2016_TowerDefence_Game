using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ProjectileRandomModel : ProjectileAI {


    [SerializeField]
    List<Mesh> models;
	static PathStart WaveManager;


    // Use this for initialization
    void Start()
    {
		// set the model as a random model in the inputed list
        GetComponent<MeshFilter>().mesh = models[Random.Range(0, models.Count)];
		WaveManager = GameObject.Find("PathPoint (0)").GetComponent<PathStart>();
    }

    // Update is called once per frame
    void Update()
    {
		//check if the current target is still alive
		if (Target != null && (Target.GetComponent("PathFollow") as PathFollow).health > 0)
		{
			//find the direction towards it
			Vector3 moveVector = Target.transform.position - this.transform.position;
			//check that the projectile wont overshoot
			if (moveVector.sqrMagnitude >= Velocity * Velocity)
				moveVector.Normalize();
			//more towards the enemy
			this.transform.Translate(moveVector * Velocity);
			Debug.DrawRay(transform.position, moveVector);

			//check if the enemy has hit the enemy
			if (Mathf.Abs(this.transform.position.x - Target.transform.position.x) < .05 &&
				Mathf.Abs(this.transform.position.y - Target.transform.position.y) < .05)
			{
				// tell the enemy if the projectil hit
				(Target.GetComponent("PathFollow") as PathFollow).health -= damage;
				Destroy(gameObject);
			}
		}
		else //if the enemy has died
		{
			//check if there are other live enemys
			if (WaveManager.LiveEnemys.Count > 0)
			{
				//find the distance to the first enemy
				float distance = (WaveManager.LiveEnemys[0].transform.position - this.transform.position).sqrMagnitude;
				int index = 0;
				//search for the closest enemy
				for(int i = 0; i < WaveManager.LiveEnemys.Count; i++)
				{
					//check the distance to the enemy to the previous closest enemy
					if(distance > (WaveManager.LiveEnemys[i].transform.position - this.transform.position).sqrMagnitude)
					{
						index = i;
						distance = (WaveManager.LiveEnemys[i].transform.position - this.transform.position).sqrMagnitude;
					}
				}
				this.Target = WaveManager.LiveEnemys[index]; // set the closest target as the current target
			}
			else
			{
				Destroy(gameObject); // if not live enemys destroy the projectil
			}
		}
    }
}
