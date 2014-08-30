using UnityEngine;
using System.Collections;

public class RandomItemGeneration : MonoBehaviour {
    public TextMesh priceDisplay;
    public ItemDictionary itemDictionary;
    public GameObject chest;
    public ItemGeneratedWindow itemGenWindow;
    public HeroLevelData heroLevel;
    int playerLevel;

    public void generate_item()
    {
        heroLevel.load_file();
        playerLevel = heroLevel.get_player_level();
        GameObject genItem = itemDictionary.generate_random_item(playerLevel);
        itemGenWindow.gameObject.SetActive(true);
        itemGenWindow.display_generated_item(genItem.GetComponent<Item>());
    }
}
