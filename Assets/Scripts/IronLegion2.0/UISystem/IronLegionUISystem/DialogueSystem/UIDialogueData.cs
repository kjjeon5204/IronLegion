using UnityEngine;
using System.Collections;


//Data containing information per character
[System.Serializable]
public struct UIDialogueSystemCharacterData
{
    public string characterName;
    public Sprite characterSprite;
}

[System.Serializable]
public struct UIDialogueSystemFullSprite
{
    public int spriteHolderRef;
    public Sprite spriteToDisplay;
}

[System.Serializable]
public struct UIDialogueSystemDialogueData
{
    public UIDialogueSystemFullSprite[] fullSprites;
    public int charAccess;
    public string dialogueText;
}


public class UIDialogueData : MonoBehaviour {
    //Setup
    public string eventID;
    public UIDialogueSystemCharacterData[] characterData;
    public UIDialogueSystemDialogueData[] dialogueData;
}
