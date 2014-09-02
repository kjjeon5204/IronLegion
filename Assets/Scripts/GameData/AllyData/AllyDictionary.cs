using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyDictionary : MonoBehaviour {
	IDictionary<string, AllyData> allyMasterDictionary;
	GameObject[] allyMasterCollection;
	
	// Use this for initialization
	void Start () {
		allyMasterCollection = Resources.LoadAll<Object>("AllyBaseData/");
		for (int ctr = 0; ctr < allyMasterCollection.Length; ctr ++) {
			AllyData temp = allyMasterCollection[ctr].GetComponent<AllyBaseData>();
			allyMasterDictionary[temp.unitName] = temp;
		}
	}
	
	public AllyData find_unit(string allyName) {
		if (allyMasterDictionary.ContainsKey(allyName))
			return allyMasterDictionary[allyName];
		else
			return null;
	}
}
