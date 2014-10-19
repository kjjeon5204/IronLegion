using UnityEngine;
using System.Collections;

public class AbilityControls : MonoBehaviour {
	private AbilityData abilities;
	private Slot[] ability_slots;
	
	public Sprite[] close_range_sprites;
	public Sprite[] long_range_sprites;
    public PlayerMasterData playerMasterData;
	
	private int saved_id;
	private DescriptionControl description;
	private GameObject library;
	
	// Use this for initialization
	void Start () {
		abilities = new AbilityData();
		abilities.load_data();

        playerMasterData.load_ability_data();
		ability_slots = new Slot[8];
		for (int i = 0; i < 8; i++)
		{
				ability_slots[i] = transform.Find("Frame"+i.ToString()).GetComponent<Slot>();
		}
		UpdateSlots();
		
		saved_id = -1;
		description = transform.Find("AbilityDescription").GetComponent<DescriptionControl>();
		library = GameObject.Find("LibraryControl");
		library.SendMessage("LoadLibrary",SendMessageOptions.DontRequireReceiver);
	}
	
	void UpdateSlots() {
		for (int i = 0; i < 8; i++)
		{
				ability_slots[i].ability_name = playerMasterData.access_hero_ability_data().ReturnAbilityName(i);
				ability_slots[i].id_num = playerMasterData.access_hero_ability_data().ReturnAbilityID(i);
				if (ability_slots[i].id_num == -1)
				{
				}
				else if (ability_slots[i].id_num < 100)
				ability_slots[i].icon_sprite = close_range_sprites[ability_slots[i].id_num];
				else
				ability_slots[i].icon_sprite = long_range_sprites[ability_slots[i].id_num-100];
		}
	}
	
	public void SetAbilityToSwitch(int id) {
		saved_id = id;
		if (id < 100)
		description.icon_sprite = close_range_sprites[id];
		else
		description.icon_sprite = long_range_sprites[id-100];
		description.ability_info = abilities.GetAbilityInfo(id);
		description.UpdateDescription();
	}
	
	public void SlotClicked(int index, bool close) {
		if ((saved_id < 100 && saved_id >= 0 && close) || (saved_id >= 100 && !close))
		{
			bool switched = playerMasterData.access_hero_ability_data().SetAbility(index,saved_id);
			if (saved_id != -1 && !switched)
			{
				//Tell person not good slot
			}
            else if (saved_id != -1)
            {
                UpdateSlots();
            }
		}
	}
	
	public Sprite GetSprite(int id) {
		if (id < 100)
		return close_range_sprites[id];
		else
		return long_range_sprites[id-100];
	}
}
