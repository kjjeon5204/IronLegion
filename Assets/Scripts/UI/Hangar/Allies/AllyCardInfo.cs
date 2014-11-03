using UnityEngine;
using System.Collections;

public class AllyCardInfo : MonoBehaviour {

	public AllyData currentAlly;
	AllyDataList dataList;
	
	public GameObject modelRotate;
	public GameObject equip;
	public GameObject equipped;
	public TextMesh cardName;
	public TextMesh description;
	public TextMesh level;
	public TextMesh hp;
	public TextMesh armor;
	public TextMesh dmg;
	bool inUse = false;
	
	public AIStatScript aiStatScript;
	
	// Use this for initialization
	void Start () {
		dataList = new AllyDataList();
		currentAlly = new AllyData();
		dataList.load_cur_equipped_ally();
		currentAlly = dataList.get_cur_equipped_ally();
		
		hp.GetComponent<Renderer>().sortingLayerName = "Tiles";
		cardName.GetComponent<Renderer>().sortingLayerName = "Tiles";
		description.GetComponent<Renderer>().sortingLayerName = "Tiles";
		armor.GetComponent<Renderer>().sortingLayerName = "Tiles";
		dmg.GetComponent<Renderer>().sortingLayerName = "Tiles";
		level.GetComponent<Renderer>().sortingLayerName = "Tiles";
		
		cardName.text = currentAlly.unitName;
		description.text = "The Minotaur mech is a heavy\narmored unit capable of taking\nmultiple hits to protect allies.";
        AIStatElement curAllyElement = aiStatScript.getLevelData(currentAlly.level);
		hp.text = curAllyElement.hp.ToString();
		armor.text = curAllyElement.baseArmor.ToString();
		dmg.text = curAllyElement.baseAttack.ToString();
		level.text = "Lvl. " + currentAlly.level.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentAlly.unitName == "MINOTAUR" && inUse == false) {
			inUse = true;
		}
		if (inUse == true) {
			equip.SetActive(false);
		}
		else {
            equip.SetActive(false);
		}
		modelRotate.transform.Rotate(Vector3.up * 10 * Time.deltaTime); 
	}
}