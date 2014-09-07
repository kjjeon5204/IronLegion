using UnityEngine;
using System.Collections;

public class RandomBoxActivation : MonoBehaviour
{
    public GameObject randomBoxAccess;
    void Clicked()
    {
        randomBoxAccess.SetActive(true);
    }
}