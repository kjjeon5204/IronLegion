using UnityEngine;
using System.Collections;



/*Input dialogue text format
 #Character reference number 0 to n where n is number of unique characters
 #Main picture reference number 0 to m where m is number of main sprites. Use -1 for picture reference.
 #Text
 
 
 
 ex.
 0
 0 1 2 3 //SpriteRenderer references
 0 1 2 4 //Load main picture reference number 0, 1, 2, and 4
 Hello World.
 
 1
 -1 //No reference needed
 -1 //This means that no main picture is used
 Hello world sucks.
 */


public class UITextDialogueData : MonoBehaviour {
    public UIDialogueSystemCharacterData[] charactersInDialogue;
    public Sprite[] mainSpriteRef;
    public TextAsset[] dialogueData; //Array will support multi language feature. 0 is dedicated to english.
}


