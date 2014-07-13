using UnityEngine;
using System.Collections;

public class MapFeatures : MonoBehaviour {
    public GameObject[] chargeFlags;
    public MapChargeFlag[] flagScripts;

	// Use this for initialization
	public void intialize_script () {
        flagScripts = new MapChargeFlag[chargeFlags.Length];
        for (int ctr = 0; ctr < chargeFlags.Length; ctr++)
        {
            flagScripts[ctr] = chargeFlags[ctr].GetComponent<MapChargeFlag>();
        }
	}
}
