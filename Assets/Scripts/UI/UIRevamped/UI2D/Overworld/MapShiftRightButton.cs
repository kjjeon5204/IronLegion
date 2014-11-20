using UnityEngine;
using System.Collections;

public class MapShiftRightButton : BaseUIButton {
    public MapChapterControl mapChapterManager;


    public override void button_released_action()
    {
        mapChapterManager.increment_chapter_ctr();
    }
}
