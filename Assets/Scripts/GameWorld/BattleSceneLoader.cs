using UnityEngine;
using System.Collections;
using System.IO;

public class BattleSceneLoader : MonoBehaviour {
    public GameObject combatScriptobject;
    public GameObject radar;
    public GameObject[] skillButtons;
    public GameObject combatScript;

	// Use this for initialization
	void Start () {
        string mapID;
        using (StreamReader inFile = File.OpenText(Application.persistentDataPath + "/MapTransferData.txt"))
        {
            mapID = inFile.ReadLine();
        }


        string mapDirectory = "BattleScene/Chapter" + mapID[1] + "/" + mapID;
        Debug.Log("Map directory: " + mapDirectory);

        GameObject holder = (GameObject)Instantiate(Resources.Load<GameObject>(mapDirectory), Vector3.zero, Quaternion.identity);
        EventControls holderScript = holder.GetComponent<EventControls>(); 
	}
}
