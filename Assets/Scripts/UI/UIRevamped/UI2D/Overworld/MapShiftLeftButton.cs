using UnityEngine;
using System.Collections;

public class MapShiftLeftButton : BaseUIButton {
    public MapChapterControl mapChapterManager;


    public override void button_released_action()
    {
        mapChapterManager.decrement_chapter_ctr();
    }
}
