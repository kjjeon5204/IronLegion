using UnityEngine;
using System.Collections;

public class ExplosionRepeat : MonoBehaviour {

	public float ExplosionTimerStart;
	public float ExplosionTimerEnd;
	public bool RandomTimeBetween;
	public GameObject Effect;
	public float SecondsUntilDeath; //leave 0 and it won't delete
	
	private float total_time; //Total time since creation
	private float random_wait_time;
	private float change_time; //Time between last instantiation, resets
	
	void Start() {
		total_time = 0f;
		change_time = 0f;
		if (ExplosionTimerEnd == 0 || !RandomTimeBetween) //Just sets range to be 0, always fixed number
		ExplosionTimerEnd = ExplosionTimerStart;
		
		random_wait_time = Random.Range(ExplosionTimerStart,ExplosionTimerEnd);
	}
	
	// Update is called once per frame
	void Update () {
		total_time += Time.deltaTime;
		change_time += Time.deltaTime;
		
		if (change_time >= random_wait_time)
		{
			change_time = 0f; //Resets time
			random_wait_time = Random.Range(ExplosionTimerStart,ExplosionTimerEnd);
			Instantiate(Effect, gameObject.transform.position, Quaternion.identity);
		}
		
		if (SecondsUntilDeath > 0 && total_time >= SecondsUntilDeath)
		{
			Destroy(gameObject);
		}
	}
}
