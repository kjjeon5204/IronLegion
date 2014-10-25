using UnityEngine;
using System.Collections;

public class RandomItemGeneration : MonoBehaviour {
    public TextMesh priceDisplay;
    public ItemDictionary itemDictionary;
    public ItemGenerateBoxControls chest;
    public ItemGeneratedWindow itemGenWindow;
    public PlayerMasterData playerMasterData;
    public float creditPrice;
    public float cogentumPrice;
    public int minLevelOffset;
    public int maxLevelOffset;
    public Renderer[] miscText;
    int playerLevel;
    bool clickComplete = false;

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

    void Update()
    {
        if (clickComplete == true)
        {
            if (!chest.run_lid())
            {
                Item generatedItem = generate_item(minLevelOffset, maxLevelOffset);
                itemGenWindow.gameObject.transform.parent.gameObject.SetActive(true);
                itemGenWindow.display_generated_item(generatedItem);
                clickComplete = false;
            }
        }
    }

    void Clicked()
    {
        if (playerMasterData.get_currency() >= creditPrice &&
            playerMasterData.get_paid_currency() >= cogentumPrice)
        {
            chest.activate_box();
            clickComplete = true;
        }
    }
}
