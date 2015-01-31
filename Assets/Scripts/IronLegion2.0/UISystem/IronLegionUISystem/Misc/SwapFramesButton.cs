using UnityEngine;
using System.Collections;

/*This button works with UI2DController. If this button is pressed, depending on the current state, 
 it turns off a group of GameObject while deactivating others.*/
public class SwapFramesButton : BaseUIButton {
    

    public GameObject[] frame1;
    public GameObject[] frame2;
    public bool oneWayActivation;

    bool swapped = false;

    void frame_switch(GameObject[] frame, bool command)
    {
        for (int ctr = 0; ctr < frame.Length; ctr++)
        {
            frame[ctr].SetActive(command);
        }
    }

    public override void button_released_action()
    {
        if (oneWayActivation || !swapped)
        {
            frame_switch(frame1, false);
            frame_switch(frame2, true);
            swapped = true;
        }
        else
        {
            frame_switch(frame1, true);
            frame_switch(frame2, false);
            swapped = false;
        }
    }
}
