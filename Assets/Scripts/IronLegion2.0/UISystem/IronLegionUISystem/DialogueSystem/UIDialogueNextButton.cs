using UnityEngine;
using System.Collections;

public class UIDialogueNextButton : BaseUIButton {
    public UIDialogueBaseSystem uiDialogueBase;


    public override void button_released_action()
    {
        uiDialogueBase.run_dialogue_system();
    }
}
