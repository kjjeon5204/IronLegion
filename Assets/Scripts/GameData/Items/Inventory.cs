using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;


public class Inventory  {
    public string[] items;
    public int numItems;
    float curRecycleProcess;
    
	public Inventory() {
	
	}
	
	public void load_inventory() {

        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;
        string rawFileData;

        using (StreamReader inFile = File.OpenText(path))
        {
            rawFileData = inFile.ReadLine();
            curRecycleProcess = (float)Convert.ToDecimal(rawFileData);
            rawFileData = inFile.ReadLine();
            numItems = Convert.ToInt32(rawFileData);
            items = new string[numItems];
            for (int ctr = 0; ctr < items.Length; ctr++)
            {
                items[ctr] = inFile.ReadLine();
            }
        }
	}


    public void store_inventory(Item[] itemList)
    {
        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(curRecycleProcess);
            outfile.WriteLine(itemList.Length);
            for (int ctr = 0; ctr < itemList.Length; ctr++)
            {
                outfile.WriteLine(itemList[ctr].itemID);
            }
        }
    }


    public void store_inventory()
    {
        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(curRecycleProcess);
            outfile.WriteLine(items.Length);
            for (int ctr = 0; ctr < items.Length; ctr++)
            {
                outfile.WriteLine(items[ctr]);
            }
        }
    }

    public void add_item(string itemID)
    {
        Array.Resize<string>(ref items, items.Length + 1);
        items[items.Length - 1] = itemID;
    }
	
	public void swap_item(int index, string itemID)
	{
		items[index] = itemID;
	}

    public float get_recycle_progress()
    {
        return curRecycleProcess;
    }

    public void save_recycle_progress(float progress)
    {
        curRecycleProcess = progress;
    }


    public string[] get_inventory()
    {
        return items;
    }
	
	public void set_inventory(string[] new_inv)
	{
		items = new_inv;
	}
}



