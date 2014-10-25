using UnityEngine;
using System.Collections;

public class DisplayCurrentStats : MonoBehaviour {
	
	public ItemControls stat_controller;
	
	public HeroLevelData hero_info;
	private Stats hero_stats;
	private Stats item_stats;
	
	public GameObject[] equipped_slots;
	private Item item_to_add;
	
	private float level;
	private float cur_xp;
	private float tot_xp;
	private float hp;
	private float armor;
	private float damage;
	private float energy;
	private float penetration;
	private float luck;
	private int currency;
	private int paid_currency;
	
	public GameObject exp_bar_fill;
	public TextMesh exp_indicator;
	public TextMesh level_indicator;
	public TextMesh hp_indicator;
	public TextMesh armor_indicator;
	public TextMesh damage_indicator;
	public TextMesh energy_indicator;
	public TextMesh penetration_indicator;
	public TextMesh luck_indicator;
	public TextMesh currency_indicator;
	public TextMesh paid_currency_indicator;
	
	private string start_color = "<b><color=lime>";
	private string end_color = "</color></b>";

    PlayerMasterData playerMasterData;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        UpdateStats();
	}
	
	public void UpdateStats() {
	
        PlayerMasterStat playerStat = hero_info.get_player_stat_all();
		level = playerStat.level;
		cur_xp = playerStat.curExp;
		tot_xp = playerStat.expRequired;
		hp = playerStat.hp;
		armor = playerStat.armor;
		damage = playerStat.damage;
		energy = playerStat.energy;
		penetration = playerStat.penetration;
		luck = playerStat.luck;
		
		currency = stat_controller.ReturnCurrency();
		paid_currency = stat_controller.ReturnPaidCurrency();
		
		UpdateImage();
	}
	
	void UpdateImage() {
        if (tot_xp > 0)
		    exp_bar_fill.transform.localScale = new Vector3(cur_xp/tot_xp,1f,1f);
		else
            exp_bar_fill.transform.localScale = new Vector3(0.0f, 1f, 1f);

		exp_indicator.text = (int)cur_xp + "<b>/</b>" + (int)tot_xp;
		level_indicator.text = start_color+"Level: "+end_color + (int)level;
		hp_indicator.text = start_color+"HP: "+end_color + (int)hp;
		if (armor >= 0)
		armor_indicator.text = start_color+"Armor: "+end_color+ (int)armor;
		else
		armor_indicator.text = start_color+"Armor: "+end_color +"0";
		damage_indicator.text = start_color+"Damage: "+end_color + (int)damage;
		energy_indicator.text = start_color+"Energy: "+end_color+ (int)energy;
		penetration_indicator.text = start_color+"Penetration: "+end_color + (int)penetration;
		luck_indicator.text = start_color+"Luck: "+end_color + (int)luck;
		currency_indicator.text = start_color+"Credits: "+end_color+ currency;
		paid_currency_indicator.text = "<b><color=purple>"+"Cogentum Ore: "+end_color+ paid_currency;
	}
	
	public float GetDamage() {
        UpdateStats();
		return damage;
	}
	
	public int GetCurrency() {
        UpdateStats();
		return currency;
	}
}
