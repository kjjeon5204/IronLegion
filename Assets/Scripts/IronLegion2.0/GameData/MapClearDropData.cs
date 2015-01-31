using UnityEngine;
using System.Collections;

/*
 This struct is used to configure both ally drop and item drop into one 
 single data structure format to send to map tile button to be displayed.*/
public class BaseDropItem
{
    public enum DropItemType
    {
        ALLY,
        ITEM
    }
    public DropItemType dropItemType;
    public Sprite spriteToDisplay;
    public GameObject itemPolled;

    public void initialize_drop_structure(GameObject objectToConvert)
    {
        Item itemRef = objectToConvert.GetComponent<Item>(); 
        AllyUnitData allyUnitData = objectToConvert.GetComponent<AllyUnitData>();
        itemPolled = objectToConvert;
        if (itemRef != null)
        {
            dropItemType = DropItemType.ITEM;
            spriteToDisplay = objectToConvert.GetComponent<SpriteRenderer>().sprite;
        }
        if (allyUnitData != null)
        {
            dropItemType = DropItemType.ALLY;
            spriteToDisplay = allyUnitData.unitSprite;
        }
    }
}


public class MapClearDropData : MonoBehaviour {
    public Item[] itemDropList;
    //putting items into these spot does NOT mean item is droppable. This is just list of key items
    //to be listed on the tile menu.
    public GameObject[] keyAllyUnitList;
    public GameObject[] keyDropItemList;
    BaseDropItem[] droppableItemList;


    /*This function is used to initialize key droppableItemList*/
    void initialize_droppable_list()
    {
        droppableItemList = new BaseDropItem[keyAllyUnitList.Length + keyDropItemList.Length];
        for (int ctr = 0; ctr < keyAllyUnitList.Length; ctr++)
        {
            droppableItemList[ctr].initialize_drop_structure(keyAllyUnitList[ctr]);
        }
        for (int ctr = 0; ctr < keyDropItemList.Length; ctr++)
        {
            droppableItemList[ctr].initialize_drop_structure(keyDropItemList[ctr]);
        }
    }

    public BaseDropItem[] get_base_drop_item()
    {
        if (droppableItemList == null)
        {
            initialize_droppable_list();
        }
        return droppableItemList;
    }

    [System.Serializable]
    public struct DropListData
    {
        public int dropWeight;
        public GameObject[] droppableItems;
    }

    /*This list contains the actual drop list for the game.*/
    public DropListData[] dropList;
}
