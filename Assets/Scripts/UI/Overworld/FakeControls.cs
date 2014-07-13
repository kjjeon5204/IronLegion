using UnityEngine;
using System.Collections;

public class FakeControls : MonoBehaviour {
	private ActivateSettings controller;
	private MapControls map;
	private AbilityControls ability;
	public int id;
	
	
	private /*ActivateAbilities*/ GameObject abilities;
	private /*ActivateAllies*/ GameObject allies;
	// Use this for initialization
	void Start () {
		id = 100;
		//controller = GameObject.Find("Settings").GetComponent<ActivateSettings>();
		//map = GameObject.Find("UI").GetComponent<MapControls>();
		ability = GameObject.Find("AbilityFrame").GetComponent<AbilityControls>();
		
		abilities = GameObject.Find("Abilities");//.GetComponent<ActivateAbilities>();
		allies = GameObject.Find("Allies");//.GetComponent<ActivateAllies>();
	}
	
	// Update is called once per frame
	void Update () {
		if (id > 103)
		id = 100;
		else if (id < 100)
		id = 103;
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
		}
		
		/*
		if (Input.GetKey(KeyCode.UpArrow))
		{
			map.ChangeCameraPosition(-1*Vector2.up);	
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			map.ChangeCameraPosition(Vector2.right);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			map.ChangeCameraPosition(Vector2.up);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			map.ChangeCameraPosition(-1*Vector2.right);
		}
		*/
	}
}
