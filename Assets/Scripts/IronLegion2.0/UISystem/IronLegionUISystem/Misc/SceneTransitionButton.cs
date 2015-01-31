using UnityEngine;
using System.Collections;

public class SceneTransitionButton : BaseUIButton {
    public LoadingScreen loadingScreen;

    public override void button_released_action()
    {
        loadingScreen.gameObject.SetActive(true);
    }
}
