using UnityEngine;
using System.Collections;

public class MapConfirmCancelButton : BaseUIButton {
    public MapEngageWindow engageWindow;
    public OverworldSceneControls ovSceneControls;

    public override void button_released_action()
    {
        engageWindow.reset_map_confirm_window();
        engageWindow.gameObject.SetActive(false);
        ovSceneControls.set_ui_state(OverworldSceneControls.UIState.MAIN);
    }
}
