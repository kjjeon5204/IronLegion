using UnityEngine;
using System.Collections;

/*This class controls all UI controllers in the scene. To use this class,
 make a derived class of this class and implement how the UI system on a particular scene will
 work by overriding "public virtual void run_ui_system()" and 
 "public virtual void disable_all_ui_system()."*/

public class SceneController : MonoBehaviour {
    public virtual void disable_all_ui_system()
    {
    }

    public virtual void run_ui_system()
    {

    }
}