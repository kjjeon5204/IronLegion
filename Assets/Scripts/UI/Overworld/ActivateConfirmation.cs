using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ActivateConfirmation : MonoBehaviour {
	private TextMesh LevelNumber;
	private TextMesh ConfirmText;
	private MapData map;
	private Camera cam;
	private GameObject camTran;
	
	private GameObject waiting;
	public GameObject onScreen;
	private MapControls controls;
	private YesConfirm yes;
	private GameObject settings;
	
	void Start () {
		LevelNumber = GameObject.Find("Level Number").GetComponent<TextMesh>();
		ConfirmText = GameObject.Find("ConfirmText").GetComponent<TextMesh>();
		
		camTran = GameObject.Find("Special Camera");
		cam = camTran.GetComponent<Camera>();
		
		waiting = GameObject.Find("Waiting Area");
		
		controls = GameObject.Find("UI").GetComponent<MapControls>();
		yes = GameObject.Find("ConfirmRight").GetComponent<YesConfirm>();
		
		settings = GameObject.Find("Settings Sprite");
	}
	
	
	public void Confirm(string mapID, int level ,string what_to_confirm)
	{
		yes.behavior = what_to_confirm;
		if (what_to_confirm == "LEVEL")
		{
			controls.confirmingLevel = true;
			ConfirmText.text = "You are attacking";
			LevelNumber.text = "Level: "+(level+1).ToString();
			//map.store_map_data(level);
            string dataPath = Application.persistentDataPath + "/MapTransferData.txt";

            using (StreamWriter outFile = File.CreateText(dataPath))
            {
                outFile.WriteLine(mapID);
            }


		}
		else if (what_to_confirm == "RESET DATA")
		{
			settings.transform.position = waiting.transform.position;
			ConfirmText.text = "Are you sure you";
			LevelNumber.text = "want to reset?";
		}
		else if (what_to_confirm == "RESET DATA CONFIRM")
		{
			settings.transform.position = waiting.transform.position;
			ConfirmText.text = "Last chance!";
			LevelNumber.text = "Reset?";
		}
		this.transform.position = onScreen.transform.position;
		//cam.enabled = true;
	}
    
    public void Confirm(string mapID, string level, string what_to_confirm)
    {
        yes.behavior = what_to_confirm;
        if (what_to_confirm == "LEVEL")
        {
            controls.confirmingLevel = true;
            ConfirmText.text = "You are attacking";
            LevelNumber.text = "Level: " + level;
            
            //map.store_map_data(level);
            string dataPath = Application.persistentDataPath + "/MapTransferData.txt";

            using (StreamWriter outFile = File.CreateText(dataPath))
            {
                outFile.WriteLine(mapID);
            }
            yes.set_level_to_load(mapID);

        }
        else if (what_to_confirm == "RESET DATA")
        {
            settings.transform.position = waiting.transform.position;
            ConfirmText.text = "Are you sure you";
            LevelNumber.text = "want to reset?";
        }
        else if (what_to_confirm == "RESET DATA CONFIRM")
        {
            settings.transform.position = waiting.transform.position;
            ConfirmText.text = "Last chance!";
            LevelNumber.text = "Reset?";
        }
        this.transform.position = onScreen.transform.position;
        //cam.enabled = true;
    }
	
	public void Reject()
	{
		controls.confirmingLevel = false;
		
		if (yes.behavior == "RESET DATA" || yes.behavior == "RESET DATA CONFIRM")
			settings.transform.position = onScreen.transform.position;
		this.transform.position = waiting.transform.position;
		//cam.enabled = false;
	}
	/*
	void Update()
	{
		leftConfirm.transform.position = transform.position + new Vector3(-4.82f,0,0);
		rightConfirm.transform.position = transform.position;
		dim.transform.position = transform.position + new Vector3(-2.6f,0,0);
	}*/
}