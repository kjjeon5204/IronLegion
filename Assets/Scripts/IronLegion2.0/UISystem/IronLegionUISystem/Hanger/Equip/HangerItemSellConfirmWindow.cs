using UnityEngine;
using System.Collections;

public class HangerItemSellConfirmWindow : MonoBehaviour {
    public TextMesh[] itemSellConfirmTexts;
    public TextMesh itemNameDisplay;
    public TextMesh itemPriceDisplay;
    public SpriteRenderer itemIconDisplay;

    public void initialize_item_sell_window(Item itemToDisplay)
    {
        itemIconDisplay.sprite = itemToDisplay.gameObject.GetComponent<SpriteRenderer>().sprite;
        itemSellConfirmTexts[0].text = "Are your sure you want to sell";
        itemNameDisplay.text = itemToDisplay.itemName;
        itemSellConfirmTexts[1].text = "for";
        itemPriceDisplay.text = itemToDisplay.sell_price.ToString();
    }
}
