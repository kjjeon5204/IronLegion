using UnityEngine;
using System.Collections;

public class AIBaseCondition : MonoBehaviour {
    public string conditionID;
    protected Character character;

    public virtual void Start()
    {
        character = GetComponent<Character>();
    }

	//Returns true if condition is met. Otherwise false
    public virtual bool check_condition()
    {
        return false;
    }
}
