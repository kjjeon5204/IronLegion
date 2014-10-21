using UnityEngine;
using System.Collections;

public class AbilityEffectControl : MonoBehaviour {
    int effectEndPhase;

    public void enable_effect(int endPhase)
    {
        gameObject.SetActive(true);
        effectEndPhase = endPhase;
    }

    public void update_effect(int curPhase)
    {
        if (curPhase >= effectEndPhase && gameObject.activeInHierarchy == true)
        {
            Debug.Log("Effect turned off!");
            gameObject.SetActive(false);
        }
    }
}
