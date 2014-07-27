using UnityEngine;
using System.Collections;

public class FakeControls : MonoBehaviour {
	private ActivateSettings controller;
	private MapControls map;
	private AbilityControls ability;
	public int id;
	public int dialogue_check = 0;
	
	private /*ActivateAbilities*/ GameObject abilities;
	private /*ActivateAllies*/ GameObject allies;
	GameObject tile_stuff;
	// Use this for initialization
	void Start () {
		id = 100;
		//controller = GameObject.Find("Settings").GetComponent<ActivateSettings>();
		//map = GameObject.Find("UI").GetComponent<MapControls>();
		//ability = GameObject.Find("AbilityFrame").GetComponent<AbilityControls>();
		
		//abilities = GameObject.Find("Abilities");//.GetComponent<ActivateAbilities>();
		//allies = GameObject.Find("Allies");//.GetComponent<ActivateAllies>();
	}
	
	// Update is called once per frame
	void Update () {
		if (id > 103)
		id = 100;
		else if (id < 100)
		id = 103;
		/*
		if (Input.GetKeyDown(KeyCode.K))
		{
			ability.SetAbilityToSwitch(id);
			id--;
			//controller.MoveSettingsOnScreen();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			ability.SetAbilityToSwitch(id);
			id++;
			//controller.MoveSettingsBack();
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			abilities.SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			allies.SendMessage("Clicked",SendMessageOptions.DontRequireReceiver);
		}*/
		
		/*
		if (Input.GetKey(KeyCode.UpArrow))
		{
			map.ChangeCameraPosition(-1*Vector2.up);	
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			map.ChangeCameraPosition(Vector2.up);
		}*/
		if (Input.GetKeyDown(KeyCode.O))
		{
			dialogue_check--;
			Debug.Log(dialogue_check);
			if (dialogue_check < 0)
			dialogue_check = 0;
			
			if (dialogue_check >= 10)
			{
				tile_stuff = GameObject.Find("0"+dialogue_check);
			}
			else 
			tile_stuff = GameObject.Find("00"+dialogue_check);
			
			tile_stuff.SendMessage("Clicked");
			//map.ChangeCameraPosition(Vector2.right);
		}
		else if (Input.GetKeyDown(KeyCode.P))
		{
			dialogue_check++;
			Debug.Log(dialogue_check);
			if (dialogue_check >= 10)
			{
				tile_stuff = GameObject.Find("0"+dialogue_check);
			}
			else 
			tile_stuff = GameObject.Find("00"+dialogue_check);
			
			tile_stuff.SendMessage("Clicked");
			//map.ChangeCameraPosition(-1*Vector2.right);
		}
		
	}
}
