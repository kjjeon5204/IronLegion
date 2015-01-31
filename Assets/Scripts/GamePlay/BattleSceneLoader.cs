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
    public CombatScript combatUI;

    // Use this for initialization
    void Start()
    {
        myCurrentBattle.initialize_script(playerMasterData);
        combatUI.initialize_ui();
    }

    void Update()
    {
        //Run Battle
        myCurrentBattle.update_combat();
        combatUI.update_ui();
    }
}