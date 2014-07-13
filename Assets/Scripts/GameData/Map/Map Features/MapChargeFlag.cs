using UnityEngine;
using System.Collections;

public class MapChargeFlag : MonoBehaviour {
    //positive attracts ai Characters
    public bool isPositive;
    //charge determines strength of the flag
    public float charge;
    //Determines the switching point when charge starts to increase
    public float suctionField;
    //Adjacent flags
    public GameObject adjacentFlags;


    public virtual void manual_start()
    {
    }

    public virtual void manual_update()
    {
    }
}
