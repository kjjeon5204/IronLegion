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
    
    public virtual void button_released_action()
    {
        
    }

    public virtual void button_pressed()
    {
        if (buttonPressed != null)
        {
            myRenderer.sprite = buttonPressed;
        }
    }

    public virtual void button_canceled()
    {

    }

    public virtual void button_held_action(Touch myTouch)
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
