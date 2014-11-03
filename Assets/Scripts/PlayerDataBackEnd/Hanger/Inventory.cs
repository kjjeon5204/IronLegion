using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;


public class Inventory  {
    public string[] items;
    public int numItems;
    public int currency;
	public int paid_currency;
    
	public Inventory() {
	
	}
	
	public void load_inventory() {

        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;
        if (!File.Exists(path))
        {
            create_new_inventory();
        }
        string rawFileData;

        using (StreamReader inFile = File.OpenText(path))
        {
            rawFileData = inFile.ReadLine();
            currency = Convert.ToInt32(rawFileData);
			rawFileData = inFile.ReadLine();
            paid_currency = Convert.ToInt32(rawFileData);
            rawFileData = inFile.ReadLine();
            numItems = Convert.ToInt32(rawFileData);
            items = new string[numItems];
            for (int ctr = 0; ctr < items.Length; ctr++)
            {
                items[ctr] = inFile.ReadLine();
            }
        }
	}

    void create_new_inventory()
    {
        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine("0");
            outfile.WriteLine("0");
            outfile.WriteLine("0");
        }
    }


    public void store_inventory(Item[] itemList)
    {
        string fileName = "/Inventory.txt";
        string path = Application.persistentDataPath + fileName;

        using (StreamWriter outfile = File.CreateText(path))
        {
            outfile.WriteLine(currency);
			outfile.WriteLine(paid_currency);
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
            outfile.WriteLine(currency);
			outfile.WriteLine(paid_currency);
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
        store_inventory();
    }
	
	public void swap_item(int index, string itemID)
	{
		items[index] = itemID;
	}

    public float get_recycle_progress()
    {
        return (float)currency;
    }

    public void save_recycle_progress(float progress)
    {
        currency = (int)progress;
    }

    public string[] get_inventory()
    {
        return items;
    }
	
	public void set_inventory(string[] new_inv)
	{
		items = new_inv;
	}
	
	public int get_currency()
	{
		return currency;
	}
	
	public int get_paid_currency() 
	{
		return paid_currency;
	}

    public void store_currency(int credit, int cogentum)
    {
        currency = credit;
        paid_currency = cogentum;
        store_inventory();
    }
	
	public void change_currency(int num)
	{
		currency += num;
        store_inventory();
	}
	
	public void change_paid_currency(int num)
	{
		paid_currency += num;
        store_inventory();
	}
}



