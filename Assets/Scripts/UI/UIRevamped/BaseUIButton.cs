using UnityEngine;
using System.Collections;

public class BaseUIButton : MonoBehaviour {
    public enum ButtonType
    {
        REGULAR,
        DRAG
    }

    public SpriteRenderer myRenderer;
    public Sprite buttonPressed;
    public Sprite buttonLifted;
    
   
    //On Button Press
    public virtual void button_pressed_action()
    {
    }

    public virtual void button_pressed(CustomInput myInput)
    {
        if (buttonPressed != null)
        {
            myRenderer.sprite = buttonPressed;
        }
    }


    //Button canceled
    public virtual void button_canceled()
    {

    }

    public virtual void button_held_action(CustomInput myInput)
    {
    }

    public virtual void button_released_action()
    {

    }

    public virtual void button_released()
    {
        if (buttonLifted != null)
        {
            myRenderer.sprite = buttonLifted;
        }
    }

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        if (buttonLifted != null)
        {
            myRenderer.sprite = buttonLifted;
        }
    }
}
