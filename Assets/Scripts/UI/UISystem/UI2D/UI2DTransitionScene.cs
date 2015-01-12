using UnityEngine;
using System.Collections;

public class UI2DTransitionScene : BaseUIButton {
    public string sceneName;
    public LoadingScreen loadingScreen;

    //Disables all UI controllers on the scene
    public SceneController sceneController;

    public override void button_released_action()
    {
        loadingScreen.set_loading_scene(sceneName);
        sceneController.disable_all_ui_system();
        loadingScreen.gameObject.SetActive(true);
    }
}
