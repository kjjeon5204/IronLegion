using UnityEngine;
using System.Collections;
using System.IO;

public class BattleSceneLoader : MonoBehaviour
{
    public GameObject combatScriptobject;
    public GameObject radar;
    public GameObject[] skillButtons;
    public GameObject combatScript;
    public PlayerMasterData playerMasterData;
    public bool developmentBuild;
    public string developmentBuildMap;
    public EventControls myCurrentBattle;

    // Use this for initialization
    void Start()
    {
        myCurrentBattle.initialize_script(playerMasterData);
    }

    void Update()
    {
        //Run Battle
        myCurrentBattle.update_combat();
    }
}