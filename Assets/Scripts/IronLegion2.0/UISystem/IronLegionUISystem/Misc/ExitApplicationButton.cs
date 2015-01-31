using UnityEngine;
using System.Collections;

public class ExitApplicationButton : BaseUIButton {

    public override void button_released_action()
    {
        Application.Quit();
    }
}
