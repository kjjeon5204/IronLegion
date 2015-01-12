using UnityEngine;
using System.Collections;

public class HangerOptionButton : BaseUIButton {
    public HangerController hangerController;
    public HangerController.HangerWindowState myWindowState;

    public override void button_released_action()
    {
        hangerController.set_window_state(myWindowState);
    }
}
