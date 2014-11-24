using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseCombatStructure : MonoBehaviour {
    protected IList<Character> enemyUnitList;
    protected IList<Character> playerUnitList;

    public IList<Character> get_list_of_enemy()
    {
        return enemyUnitList;
    }

    protected MainChar playerScript;

    public virtual void initialize_combat(MainChar playerScriptInput)
    {
    }

    public virtual void update_battle()
    {
    }
}
