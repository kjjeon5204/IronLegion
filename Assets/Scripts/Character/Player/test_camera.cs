/*
using UnityEngine;
using System.Collections;

public class test_camera : MonoBehaviour {
	GameObject MyCam;
	public GameObject[] myCamList;
	Camera[] myCamAccess;
	Vector3 originalposition;
	public GameObject player;
	GameObject target;
	public Vector3 direction;
	public CharacterAttackType curAttack;
	public string curattacttype;
	public int count = 0;
	Vector3 targposition;

	public enum Camera_State
	{
		IDLE,
		BEAM,
		SLASH,
		GATLING,
		SHATTER,
		BLUT,
		RIFLE
	}

	Camera_State curState;

	float smooth = 5;
	float stopTime;
	private bool BEAM = false;
	private bool SLASH = false;
	private bool GATLING = false;
	private bool SHATTER = false;
	private bool BLUT = false;
	private bool RIFLE = false;

	// Use this for initialization
	void Start () {
		MyCam = gameObject.transform.gameObject;

		myCamAccess = new Camera[myCamList.Length];
		for (int ctr = 0; ctr < myCamAccess.Length; ctr ++) {
			myCamAccess[ctr] = myCamList[ctr].GetComponent<Camera>();
		}

		myCamAccess [0].enabled = true;
		myCamAccess [1].enabled = false;
		originalposition = MyCam.transform.localPosition;
		curState = Camera_State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		MainChar playerscript = player.GetComponent<MainChar>();
		curAttack = playerscript.curAttack;
		target = playerscript.target;
		curattacttype = curAttack.attackID;
	 	event_handle();
//		if (target == null) {
//			count = 0;
//			curState = Camera_State.IDLE;
//			target = playerscript.target;
//		}

		if (curState != Camera_State.IDLE && count == 0)
		{
			targposition = target.transform.position;
			MyCam.transform.localPosition = originalposition;
			MyCam.transform.LookAt(player.transform);
			stopTime = Time.time;
			myCamAccess [1].enabled = true;
			myCamAccess [0].enabled = false;
			curState = Camera_State.IDLE;
			count = 1;
		}
	
		if (BEAM == true){ // "r"
			if (count == 1 )
			{
				MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(- 2f * smooth * Vector3.right * Time.deltaTime);
				if (stopTime + 0.8f < Time.time)
				{
					count = 2 ;
				}
			}
			if (count == 2)
			{
				MyCam.transform.LookAt(targposition);
				MyCam.transform.Translate(1.3F * smooth * Vector3.forward * Time.deltaTime);
				if (stopTime + 2.0f < Time.time)
				{
					count = 3;
				}
			}
			if (count == 3)
			{
				MyCam.transform.LookAt(targposition);
				MyCam.transform.Translate(2f * smooth * Vector3.right * Time.deltaTime);
				if (stopTime + 3.0f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 4;
				}
			}
			if (count == 4)
			{
				if(curAttack.attackID == "idle")
				{
					BEAM = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
		else if(SLASH == true){ // "f"
			if (count == 1 )
			{
				//MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(0.2f * smooth * Vector3.back * Time.deltaTime);
				if (stopTime + 0.9f < Time.time)
				{
					count = 2 ;
				}
			}
			if (count == 2 )
			{
				//MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(1.2f * smooth * Vector3.right * Time.deltaTime);
				if (stopTime + 2.5f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 3 ;
				}
			}

			if (count == 3)
			{
				if(curAttack.attackID == "idle")
				{
					SLASH = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
		else if(GATLING == true){ // "f"
			if (count == 1)
			{
				MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(smooth * Vector3.right * Time.deltaTime);
				if (stopTime + 3.0f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 2;
				}
			}
			if (count == 2)
			{
				if(curAttack.attackID == "idle")
				{
					GATLING = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
		else if(SHATTER == true){ // "d"
			if (count == 1)
			{
				MyCam.transform.LookAt(player.transform);
				//MyCam.transform.Translate(2f * smooth * Vector3.left * Time.deltaTime);
				MyCam.transform.Translate(0.15f * smooth * Vector3.back * Time.deltaTime);
				if (stopTime + 3.0f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 2;
				}
			}
			if (count == 2)
			{
				if(curAttack.attackID == "idle")
				{
					SHATTER = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
		else if(BLUT == true){ // "s"
			if (count == 1 )
			{
				MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(1.2f * smooth * Vector3.right * Time.deltaTime);
				if (stopTime + 0.9f < Time.time)
				{
					count = 2 ;
				}
			}
			if (count == 2)
			{
				MyCam.transform.LookAt(player.transform);
				//MyCam.transform.Translate(1.2f * smooth * Vector3.right * Time.deltaTime);
				MyCam.transform.Translate(0.3f * smooth * Vector3.back * Time.deltaTime);
				if (stopTime + 3.0f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 3;
				}
			}
			if (count == 3)
			{
				if(curAttack.attackID == "idle")
				{
					BLUT = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
		else if(RIFLE == true){ // "Q"
			if (count == 1)
			{
				MyCam.transform.LookAt(player.transform);
				MyCam.transform.Translate(0.1f * smooth * Vector3.down * Time.deltaTime);
				if (stopTime + 1.5f < Time.time)
				{
					myCamAccess [0].enabled = true;
					myCamAccess [1].enabled = false;
					count = 2;
				}
			}
			if (count == 2)
			{
				if(curAttack.attackID == "idle")
				{
					RIFLE = false;
					count = 0;
					curState = Camera_State.IDLE;
				}
			}
		}
	}

	void event_handle()
	{
		if (curState == Camera_State.IDLE)
		{
			if (curAttack.attackID == "Beam")//R
			{
				MyCam.transform.LookAt(player.transform);
				//curState = Camera_State.BEAM;
				BEAM = true;
			}
			if (curAttack.attackID == "Slash")//F
			{
				curState = Camera_State.SLASH;
				SLASH = true;
			}
			if (curAttack.attackID == "Gatling")//A
			{
				curState = Camera_State.GATLING;
				GATLING = true;
			}
			if (curAttack.attackID == "Shatter")//D
			{
				curState = Camera_State.SHATTER;
				SHATTER = true;
			}
			if (curAttack.attackID == "Blutsauger")//B
			{
				curState = Camera_State.BLUT;
				BLUT = true;
			}
			if (curAttack.attackID == "Rifleshot")//B
			{
				curState = Camera_State.RIFLE;
				RIFLE = true;
			}
		}
	}
}
*/
