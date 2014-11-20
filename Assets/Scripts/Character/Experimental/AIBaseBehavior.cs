using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct AIModuleContainer
{
	public string containerID;
    public string moduleID;
    public string conditionID;
}

[System.Serializable]
public struct AIBehaviorModuleNode
{
	public string nodeID;
    public string currentModuleContainer; //refers to ModuleContainerID
    public string[] nextModuleData; //Refers to next node ID
}

public class AIBaseBehavior : Character {
	public AIModuleContainer[] moduleContainers;
	public AIBehaviorModuleNode[] moduleNodes;
	AIBehaviorModuleNode curState; //Refers to current nodeID
	AIBaseModule currentModule;

    IDictionary<string, AIBaseModule> moduleLibrary = new Dictionary<string, AIBaseModule>();
    IDictionary<string, AIBaseCondition> conditionLibrary = new Dictionary<string, AIBaseCondition>();
	IDictionary<string, AIBehaviorModuleNode> moduleNodesLibrary = new Dictionary<string, AIBehaviorModuleNode>();
	IDictionary<string, AIModuleContainer> moduleContainerLibrary = new Dictionary<string, AIModuleContainer>();

	AIBaseModule access_module(string moduleName) {
		return moduleLibrary[moduleName];
	}

	AIBaseModule access_module(AIModuleContainer getContainerData) {
		return moduleLibrary[getContainerData.moduleID];
	}

	AIBaseModule access_module(AIBehaviorModuleNode getNodeData) {
		return moduleLibrary[moduleContainerLibrary[getNodeData.currentModuleContainer].moduleID];
	}

	AIBaseCondition access_condition(string conditionName) {
		return conditionLibrary[conditionName];
	}

	AIBaseCondition access_condition(AIModuleContainer getContainerData) {
		return conditionLibrary[getContainerData.conditionID];
	}

	AIBaseCondition access_condition(AIBehaviorModuleNode getNodeData) {
		return conditionLibrary[moduleContainerLibrary[getNodeData.currentModuleContainer].conditionID];
	}
	

	// Use this for initialization
	public override void manual_start () {
		base.manual_start();
        //Initialize module library
        AIBaseModule[] temp = GetComponents<AIBaseModule>();
        for (int ctr = 0; ctr < temp.Length; ctr++) {
            moduleLibrary[temp[ctr].moduleID] = temp[ctr];
        }

        AIBaseCondition[] conditionHolder = GetComponents<AIBaseCondition>();
        for (int ctr = 0; ctr < conditionHolder.Length; ctr++)
        {
            conditionLibrary[conditionHolder[ctr].conditionID] = conditionHolder[ctr]; 
        }

		for (int ctr = 0; ctr < moduleContainers.Length; ctr++) {
			moduleContainerLibrary[moduleContainers[ctr].containerID] = moduleContainers[ctr];
		}

		for (int ctr = 0; ctr < moduleNodes.Length; ctr ++) {
			moduleNodesLibrary[moduleNodes[ctr].nodeID] = moduleNodes[ctr];
		}

		curState = moduleNodes[0];
		currentModule = access_module(moduleNodes[0]);
		currentModule.initialize_module();
	}

	public AIBehaviorModuleNode get_next_available_node(string[] node) {
		AIModuleContainer curContainer;
		for (int ctr = 0; ctr < node.Length; ctr++) {
			if (moduleContainerLibrary[moduleNodesLibrary[node[ctr]].currentModuleContainer].conditionID == "" || 
			    access_condition(moduleNodesLibrary[node[ctr]]).check_condition()) {
				return moduleNodesLibrary[node[ctr]];
			}
		}
		return moduleNodes[0];
	}

    // Update is called once per frame
    public override void manual_update()
	{
		Debug.Log ("Current Module: " + curState.nodeID);
		if (currentModule.run_module() == false) {
			curState = get_next_available_node(curState.nextModuleData);
			currentModule = access_module(curState);
			currentModule.initialize_module();
		}
	}
}
