using UnityEngine;
using System.Collections;

public class RandomItemGeneration : MonoBehaviour {
    public TextMesh priceDisplay;
    public ItemDictionary itemDictionary;
    public ItemGenerateBoxControls chest;
    public ItemGeneratedWindow itemGenWindow;
    public PlayerMasterData playerMasterData;
    public ArmoryControl armoryControls;
    public int creditPrice;
    public int cogentumPrice;
    public int minLevelOffset;
    public int maxLevelOffset;
    public Renderer[] miscText;
    int playerLevel;
    public Collider2D[] otherBoxColliders;
    bool clickComplete = false;

    void OnEnable()
    {
        collider2D.enabled = true;
    }

    Item generate_item(int positiveOffset, int negativeOffset)
    {
        playerLevel = playerMasterData.get_player_level();
        negativeOffset += playerLevel;
        positiveOffset += playerLevel;
        if (negativeOffset < 0)
        {
            negativeOffset = 0;
        }
        if (negativeOffset > 15)
        {
            negativeOffset = 15;
        }
        if (positiveOffset > 15)
        {
            positiveOffset = 15;
        }
        if (positiveOffset < 0)
        {
            positiveOffset = 0;
        }
        int itemGenLevel = 0;
        if (negativeOffset > positiveOffset)
        {
            int temp = positiveOffset;
            positiveOffset = negativeOffset;
            positiveOffset = temp;
        }
        if (negativeOffset < positiveOffset)
        {
            itemGenLevel = Random.Range(negativeOffset, positiveOffset);
        }
        if (negativeOffset >= 15 && positiveOffset >= 15)
        {
            itemGenLevel = 15;
        }
        GameObject genItem = itemDictionary.generate_random_item(itemGenLevel);
        return genItem.GetComponent<Item>();
    }

    void Start()
    {
        if (creditPrice == 0)
        {
            priceDisplay.text = cogentumPrice.ToString();
        }
        else
        {
            priceDisplay.text = creditPrice.ToString();
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
            armoryControls.credit_spent(creditPrice, cogentumPrice);

            for (int ctr = 0; ctr < otherBoxColliders.Length; ctr++)
            {
                otherBoxColliders[ctr].enabled = false;
            }
            chest.activate_box();
            clickComplete = true;
        }
    }
}
