using UnityEngine;
using System.Collections;

public class StatDisplay : MonoBehaviour {
    public GameObject HP_Text;
    public GameObject Armor_Text;
    public GameObject Damage_Text;
    public GameObject Level_Text;
    public GameObject Exp_Text;

    public void setText(Item input)
    {
        HP_Text.GetComponent<TextMesh>().text = input.hp.ToString();
        Armor_Text.GetComponent<TextMesh>().text = ((int)input.armor).ToString();
        Damage_Text.GetComponent<TextMesh>().text = ((int)input.damage).ToString();
    }


    public void setText(Stats input)
    {
        HP_Text.GetComponent<TextMesh>().text = input.baseHp.ToString();
        Armor_Text.GetComponent<TextMesh>().text = ((int)input.armor).ToString();
        Damage_Text.GetComponent<TextMesh>().text = ((int)input.baseDamage).ToString();
        Level_Text.GetComponent<TextMesh>().text = "LV: " + (input.level).ToString();
        Exp_Text.GetComponent<TextMesh>().text = (input.curExp) + "/" + (input.totalExp);
    }

    public void setEmpty()
    {
        HP_Text.GetComponent<TextMesh>().text = "0";
        Armor_Text.GetComponent<TextMesh>().text = "0";
        Damage_Text.GetComponent<TextMesh>().text = "0";
    }


	// Use this for initialization
	void Start () {
        HP_Text.GetComponent<TextMesh>().text = "0";
        Armor_Text.GetComponent<TextMesh>().text = "0";
        Damage_Text.GetComponent<TextMesh>().text = "0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
