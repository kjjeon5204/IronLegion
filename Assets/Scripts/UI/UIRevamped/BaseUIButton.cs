using UnityEngine;
using System.Collections;

public class BasUiCutton : MonoBehaviour {
    public Sprite buttonPressed;
    public Sprite buttonLifted;
    
    public virtual void button_action()
    {
    }

    public void button_pressed()
    {
    }

    public void button_canceled()
    {
    }

    public void button_released()
    {
    }
}
