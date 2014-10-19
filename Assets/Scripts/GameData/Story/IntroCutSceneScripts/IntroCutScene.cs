using UnityEngine;
using System.Collections;


[System.Serializable]
public struct IntroStorySequence
{
    public Sprite backgroundSprite;
    public string dialogueText;
}

public class IntroCutScene : MonoBehaviour {
    public Camera sceneCam;
    public Camera GUICam;
    


    public IntroStorySequence[] myStory;
    int storyPhase = 0;

    public bool storyTextSequence = true;
    public float textAppearTime;
    public float textRemainTime;
    public float textFadeTime;
    int textPhase;


    public SpriteRenderer pic2TextBox;
    public SpriteRenderer backgroundPicture;
    public TextMesh textAppearance;
    public GameObject skipButton;
    float timeTracker = 0.0f;
    float timeTracker2 = 0.0f;
    float timePerCharacter;
    string currentTextToDisplay;
    int currentCharacterTracker = 0;
    bool textCompleted;

    int currentStorySequence = 0;

    float sizeChangeRate;
    float camMoveTracker;
    Vector3 camPanMovement;
    Vector3 pic2CamStartPos;
    Vector3 pic3CamStartPos;
    bool sceneFadeIn;
    bool sceneFadeOut;
    int touchCount = 0;
    


    public void text_start()
    {
        Color tempColorHolder = textAppearance.color;
        tempColorHolder.a = 1.0f;
        textAppearance.color = tempColorHolder;
        currentCharacterTracker = 0;
        currentTextToDisplay = myStory[currentStorySequence].dialogueText;
        timePerCharacter = 1.0f / 40.0f;
        timeTracker = 0.0f;
        textCompleted = false;
        textPhase++;
    }

    public void intro_sequence_text_appear_phase()
    {
        timeTracker += Time.deltaTime;
        int curDisplayedTexts = (int) (timeTracker / timePerCharacter);
        if (curDisplayedTexts > currentTextToDisplay.Length)
        {
            curDisplayedTexts = currentTextToDisplay.Length;
            timeTracker = Time.time + 3.0f;
            textPhase++;
            textCompleted = true;
        }
        textAppearance.text = new string(currentTextToDisplay.ToCharArray(), 
            0, curDisplayedTexts);
    }

    public void intro_sequence_text_stay()
    {
        if (timeTracker < Time.time) {
            //Conditions to move to text fade
            timeTracker = Time.time + 1.0f;
            textPhase++;
        }
    }

    public bool intro_sequence_text_fade()
    {
        if (timeTracker > Time.time)
        {
            Color tempColorHolder = textAppearance.color;
            tempColorHolder.a -= 1.0f * Time.deltaTime;
            textAppearance.color = tempColorHolder;
            return true;
        }
        else
        {
            return false;
        }
    }

	// Use this for initialization
	void Start () {
        sizeChangeRate = 2.0f / 16.0f;
        camMoveTracker = Time.time + 16.0f;
        camPanMovement = transform.position / 16.0f;
        camPanMovement.z = 0.0f;
        timeTracker = Time.time + 0.5f;
	}

	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            Vector3 touchPos = GUICam.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hitButton = Physics2D.Raycast(touchPos, Vector3.forward, 100.0f);
            if (hitButton.collider != null)
            {
                if (hitButton.collider.gameObject == skipButton)
                {
                    Application.LoadLevel("Cinematic_OP_1");
                }
            }
        }
        
        if (storyTextSequence == true)
        {
            if (currentStorySequence < myStory.Length)
            {
                if ((currentStorySequence == 0 || currentStorySequence == 1) &&
                    Time.time < camMoveTracker)
                {
                    sceneCam.orthographicSize += sizeChangeRate * Time.deltaTime;
                    transform.position -= camPanMovement * Time.deltaTime;
                }
                if ((currentStorySequence == 2 || currentStorySequence == 3) && 
                    Time.time < camMoveTracker)
                {
                    transform.position -= camPanMovement * Time.deltaTime;
                }
                if ((currentStorySequence == 4 || currentStorySequence == 5) &&
                    Time.time < camMoveTracker)
                {
                    sceneCam.orthographicSize += sizeChangeRate * Time.deltaTime;
                    transform.position -= camPanMovement * Time.deltaTime;
                }
                if (textPhase == 0)
                {
                    textAppearance.text = null;
                    backgroundPicture.sprite = myStory[currentStorySequence].backgroundSprite;
                    if (Time.time < timeTracker)
                    {

                        Color tempColorHolder = pic2TextBox.color;
                        tempColorHolder.a = 0.0f;
                        pic2TextBox.color = tempColorHolder;
                        tempColorHolder = backgroundPicture.color;
                        tempColorHolder.a += 2.0f * Time.deltaTime;
                        if (tempColorHolder.a > 1.0f)
                            tempColorHolder.a = 1.0f;
                        backgroundPicture.color = tempColorHolder;
                    }
                    else
                    {
                        
                        if (currentStorySequence == 2 && timeTracker2 > Time.time)
                        {
                            Color tempColorHolder = pic2TextBox.color;
                            tempColorHolder.a += 2.0f * Time.deltaTime;
                            if (tempColorHolder.a > 1.0f)
                                tempColorHolder.a = 1.0f;
                            pic2TextBox.color = tempColorHolder;
                        }
                        else 
                        {
                            if (currentStorySequence == 2 || currentStorySequence == 4)
                                camMoveTracker = Time.time + 15.0f;
                            textPhase++;
                        }
                    }
                }
                else if (textPhase == 1)
                {
                    text_start();
                }
                else if (textPhase == 2)
                {
                    intro_sequence_text_appear_phase();
                }
                else if (textPhase == 3)
                {
                    intro_sequence_text_stay();
                }
                else if (textPhase == 4)
                {
                    if (!intro_sequence_text_fade())
                    {
                        if (currentStorySequence == 1 || currentStorySequence == 3)
                        {
                            textPhase++;
                            timeTracker = Time.time + 0.5f;
                        }
                        else
                        {
                            timeTracker = Time.time + 0.5f;
                            textPhase = 1;
                            currentStorySequence++;
                        }
                    }
                }
                else if (textPhase == 5)
                {
                    if (Time.time < timeTracker)
                    {
                        Color tempColorHolder = backgroundPicture.color;
                        tempColorHolder.a -= 2.0f * Time.deltaTime;
                        if (tempColorHolder.a < 0.0f)
                            tempColorHolder.a = 0.0f;
                        backgroundPicture.color = tempColorHolder;
                    }
                    else
                    {

                        currentStorySequence++;
                        if (currentStorySequence == 2)
                        {
                            //Starting cam conditions for 2nd pic
                            transform.position = new Vector3(2.2f, 0.0f, -10.0f);
                            camPanMovement = new Vector3(4.0f, 0.0f, 0.0f) / 15.0f;
                            sceneCam.orthographicSize = 4.5f;
                            camMoveTracker = Time.time + 15.0f;
                        }
                        if (currentStorySequence == 4)
                        {
                            //Starting cam conditions for 3rd pic
                            transform.position = new Vector3(-2.919f, -1.0478f, -10.0f);
                            camPanMovement = transform.position / 15.0f;
                            camPanMovement.z = 0.0f;
                            sceneCam.orthographicSize = 4.0f;
                            sizeChangeRate = 1.0f / 15.0f;
                            camMoveTracker = Time.time + 15.0f;
                        }
                        timeTracker2 = Time.time + 1.0f;
                        timeTracker = Time.time + 0.5f;
                        textPhase = 0;
                    }
                }
            }
            else
            {
                storyTextSequence = false;
                Application.LoadLevel("Cinematic_OP_1");
            }
        }
	}
}
