using UnityEngine;
using System.Collections;

public class DisplayCurrentStats : MonoBehaviour {
	
	public HeroLevelData hero_info;
	private Stats hero_stats;
	private HeroStats hero;
	private PlayerStat item_stats;

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
	}
}
