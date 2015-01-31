using UnityEngine;
using System.Collections;



public class UIDialogueBaseSystem : MonoBehaviour {
    //Setup
    public UIDialogueData dialogueData;
    UIDialogueSystemDialogueData[] dialogueRunData;
    public UI2DController dialogueSystemInputController;
    public SpriteRenderer fullTextBox;
    public TextMesh characterNameDisplay;
    public TextMesh[] fullTextDisplay;
    public TextMesh textBuffer;
    public float characterDisplaySpeed;

    //Dialogue run data
    int dialogueProgress;



    /*This function returns true if dialogue system is ready to run.
     This function returns false in case there was an interruption in initialization
     or the dialogue has already been played before. If this function returns false,
     user should not be able to continue to play dialogue.*/
    public virtual bool initialize_dialogue_system(/*UIDialogueData dialogueData*/)
    {
        if (dialogueData == null) Debug.Log("Null dialogue data!");
        dialogueRunData = dialogueData.dialogueData;
        wordDisplaying = false;
        return true;
    }

    
    bool wordDisplaying = false;
    string wordToDisplay;
    int curWordDisplayProcess;
    int currentLineDisplay;

    /*Transfer word*/
    IEnumerator transfer_word_text()
    {
        wordDisplaying = true;
        curWordDisplayProcess = 0;
        while (curWordDisplayProcess < wordToDisplay.Length)
        {
            fullTextDisplay[currentLineDisplay].text += wordToDisplay[curWordDisplayProcess];
            curWordDisplayProcess++;
            yield return new WaitForSeconds(characterDisplaySpeed);
        }
        wordDisplaying = false;
    }

    bool sentenceDisplayProcess;
    string sentenceToDisplay;
    int wordCtr;

    /*This function is used to check whether or not next word will fit onto current textbox
     If the word fits, returns true, otherwise, returns false. Before this function returns
     false, the function increments the currentLineDisplay by 1.*/
    bool check_word_fit(string tempWord)
    {
        //store into text buffer
        textBuffer.text = tempWord;
        //Move text buffer to the latest character position
        Vector3 positionShifter = fullTextDisplay[currentLineDisplay].renderer.bounds.extents + textBuffer.renderer.bounds.extents;
        positionShifter.z = 0;
        textBuffer.gameObject.transform.position = fullTextDisplay[currentLineDisplay].renderer.bounds.center
            + positionShifter;
        //Check to see if right extent of buffer goes past right extent of text box.
        if (textBuffer.renderer.bounds.extents.x + textBuffer.transform.position.x >
            fullTextBox.renderer.bounds.center.x + fullTextBox.renderer.bounds.extents.x)
        {
            currentLineDisplay++;
            //Check if this is a valid line
            Debug.Log("Change Line!");
            return false;
        }
        
        return true;
    }

    IEnumerator full_text_display_process()
    {
        sentenceDisplayProcess = true;
        string[] listOfWords = sentenceToDisplay.Split(' ');
        while (wordCtr < listOfWords.Length)
        {
            if (!wordDisplaying)  //Check to see if next word is ready to be processed
            {
                //check if next word fits
                if (check_word_fit(listOfWords[wordCtr]))
                {
                    fullTextDisplay[currentLineDisplay].text += " ";
                }
                //set next word to display
                wordToDisplay = listOfWords[wordCtr]; 
                StartCoroutine(transfer_word_text());
            }

            while (wordDisplaying) //Wait until currently set word is fully displayed
            {
                yield return null;
            }
            wordCtr++;
        }
        sentenceDisplayProcess = false;
    }

    public void skip_sentence_display_process()
    {
        StopCoroutine("transfer_word_text");
        StopCoroutine("full_text_display_process");
        fullTextDisplay[currentLineDisplay].text = sentenceToDisplay;
    }


    bool sentenceMessageInitialized;

    void empty_lines()
    {
        for (int ctr = 0; ctr < fullTextDisplay.Length; ctr++)
        {
            fullTextDisplay[ctr].text = "";
        }
    }

    /*This function returns false while the dialogue system is running and returns
     true when dialogue system stops running. It is recommended that you let this function
     to continue run until it returns true. Running this system to end will cause the dialogue
     system to clean itself up and shut down.*/
    public virtual bool run_dialogue_system()
    {
        if (dialogueProgress < dialogueRunData.Length)
        {
            if (!sentenceDisplayProcess)
            {
                //Initialize display next text method
                empty_lines();
                currentLineDisplay = 0;
                wordCtr = 0;
                sentenceToDisplay = dialogueRunData[dialogueProgress].dialogueText;
                StartCoroutine(full_text_display_process());
                dialogueProgress++; //Prepare for next sentence
            }
            return false;
        }
        return true;
    }


    /*This function is used in case dialogue system needs to shut itself down in the middle of the
     dialogue.*/
    public virtual void shut_down_dialogue_system()
    {

    }

    //For debugging purposes.
    void Start()
    {
        initialize_dialogue_system();
    }
}
