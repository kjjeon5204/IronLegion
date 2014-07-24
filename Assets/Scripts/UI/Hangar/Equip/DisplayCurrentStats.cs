using UnityEngine;
using System.Collections;

public class DisplayCurrentStats : MonoBehaviour {
	
	public HeroLevelData hero_info;
	private Stats hero_stats;
	private HeroStats hero;
	private PlayerStat item_stats;
	
	private float level;
	private float cur_xp;
	private float tot_xp;
	private float hp;
	private float armor;
	private float damage;
	private float energy;
	private float penetration;
	private float luck;

	// Use this for initialization
	void Start () {
		hero_info.load_file();
		hero = new HeroStats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void UpdateStats() {
		hero_stats = hero_info.get_player_stat_all();
		hero.load_data();
		item_stats = hero.get_item_stats();
		
		level = hero_stats.level;
		cur_xp = hero_stats.curExp;
		tot_xp = hero_stats.totalExp;
		hp = hero_stats.baseHp+item_stats.item_hp;
		armor = item_stats.item_armor;
		damage = item_stats.item_damage;
		energy = hero_stats.item_energy + item_stats.item_energy;
		penetration = item_stats.item_penetration;
		luck = item_stats.item_luck;
	}
}
