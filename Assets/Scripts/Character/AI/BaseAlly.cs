using UnityEngine;
using System.Collections;

public class BaseAlly : Character {
    protected Vector3 movementPosition;

    public void set_target(Character inputTarget)
    {
        targetScript = inputTarget;
        target = targetScript.gameObject;
    }

    public void set_movement_position(Vector3 movePos)
    {
        movementPosition = movePos;
    }
}