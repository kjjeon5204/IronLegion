using UnityEngine;
using System.Collections;


[System.Serializable]
public struct DialogueData
{
    public string dialogueDescription;
    public Sprite characterSprite;
    public string characterName;
    public string text;
    public float textStartTime;
    public float textDuration;
}


public class CombatDialogue : MonoBehaviour {
    public DialogueData[] dialogues;

}
