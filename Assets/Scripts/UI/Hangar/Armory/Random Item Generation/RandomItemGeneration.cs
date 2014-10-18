using UnityEngine;
using System.Collections;

public class RandomItemGeneration : MonoBehaviour {
    public TextMesh priceDisplay;
    public ItemDictionary itemDictionary;
    public GameObject chest;
    public ItemGeneratedWindow itemGenWindow;
    public PlayerMasterData playerMasterData;
    public int minLevelOffset;
    public int maxLevelOffset;
    public Renderer[] miscText;
    int playerLevel;



    Item generate_item(int positiveOffset, int negativeOffset)
    {
        if (playerLevel + negativeOffset < 0)
        {
            negativeOffset = 0;
        }
        int itemGenLevel = Random.Range(negativeOffset, positiveOffset);
        playerLevel = playerMasterData.get_player_level();
        itemGenLevel += playerLevel;
        GameObject genItem = itemDictionary.generate_random_item(itemGenLevel);
        return genItem.GetComponent<Item>();
    }

    void Start()
    {
        foreach (Renderer textRenderer in miscText)
        {
            textRenderer.sortingLayerName = "RandomBox";
        }
    }

    void Clicked()
    {
        Item generatedItem = generate_item(minLevelOffset, maxLevelOffset);
        itemGenWindow.gameObject.SetActive(true);
        //Debug.Log("Currently generated" + generatedItem.itemID);
        itemGenWindow.display_generated_item(generatedItem);
    }
}
