using UnityEngine;
using System.Collections;

public class Booster : MonoBehaviour {
    float rotSpeed = 90.0f;
    public GameObject[] boosterLengthController;
    float[] yScale;
    public float boosterChangeRate;
    float[] boosterPercentage;
    bool boosterOn;
    public Vector3 scaleDirection;

    

    public void turn_on_booster(float boosterPercentageIncrease)
    {
        boosterOn = true;
        if (boosterPercentage != null)
        {
            for (int ctr = 0; ctr < boosterPercentage.Length; ctr++)
            {
                boosterPercentage[ctr] += boosterPercentageIncrease;
            }
        }
    }

    public void instant_thruster(float scale)
    {
        //Stretch based on y axis
        if (scaleDirection == Vector3.up)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {

                Vector3 curScale = boosterLengthController[ctr].transform.localScale;
                curScale.y = yScale[ctr] * scale;
                boosterLengthController[ctr].transform.localScale = curScale;
            }
        }

        //stretch based on z axis
        if (scaleDirection == Vector3.forward)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {
                Vector3 curScale = boosterLengthController[ctr].transform.localScale;
                curScale.z = yScale[ctr] * 3.0f;
                boosterLengthController[ctr].transform.localScale = curScale;
            }
        }
    }

    public void shut_down_booster()
    {
        for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
        {
            boosterLengthController[ctr].SetActive(false);
        }
    }

    public void turn_off_booster()
    {
        boosterOn = false;
        if (boosterPercentage != null)
        {
            for (int ctr = 0; ctr < boosterPercentage.Length; ctr++)
            {
                boosterPercentage[ctr] = 0.0f;
            }
        }
    }


    void Start()
    {
        yScale = new float[boosterLengthController.Length];
        for (int ctr = 0; ctr < yScale.Length; ctr++)
        {
            Vector3 curScale = Vector3.zero;
            if (scaleDirection == Vector3.up)
            {
                yScale[ctr] = boosterLengthController[ctr].transform.localScale.y;
                curScale = boosterLengthController[ctr].transform.localScale;

                curScale.y = 0.0f;
            }
            else if (scaleDirection == Vector3.forward)
            {
                yScale[ctr] = boosterLengthController[ctr].transform.localScale.z;
                curScale = boosterLengthController[ctr].transform.localScale;

                curScale.z = 0.0f;
            }
            boosterLengthController[ctr].transform.localScale = curScale;
            boosterPercentage = new float[boosterLengthController.Length];
        }
        boosterOn = false;
    }

    void booster_on()
    {
        //Stretch based on y axis
        if (scaleDirection == Vector3.up)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {
                
               Vector3 curScale = boosterLengthController[ctr].transform.localScale;
               curScale.y += boosterChangeRate * Time.deltaTime;
               boosterLengthController[ctr].transform.localScale = curScale;
            }
        }

        //stretch based on z axis
        if (scaleDirection == Vector3.forward)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {
               Vector3 curScale = boosterLengthController[ctr].transform.localScale;
               curScale.z += boosterChangeRate * Time.deltaTime;
               boosterLengthController[ctr].transform.localScale = curScale;
            }
        }
    }

    void booster_off()
    {
        if (scaleDirection == Vector3.up)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {
                if (boosterLengthController[ctr].transform.localScale.y > 0.0f)
                {
                    Vector3 curScale = boosterLengthController[ctr].transform.localScale;
                    curScale.y -= boosterChangeRate * Time.deltaTime;
                    boosterLengthController[ctr].transform.localScale = curScale;
                    if (boosterLengthController[ctr].transform.localScale.y < 0.0f)
                    {
                        curScale.y = 0.0f;
                        boosterLengthController[ctr].transform.localScale = curScale;
                    }
                }
            }
        }
        else if (scaleDirection == Vector3.forward)
        {
            for (int ctr = 0; ctr < boosterLengthController.Length; ctr++)
            {
                if (boosterLengthController[ctr].transform.localScale.z > 0.0f)
                {
                    Vector3 curScale = boosterLengthController[ctr].transform.localScale;
                    curScale.z -= boosterChangeRate * Time.deltaTime;
                    boosterLengthController[ctr].transform.localScale = curScale;
                    if (boosterLengthController[ctr].transform.localScale.z < 0.0f)
                    {
                        curScale.z = 0.0f;
                        boosterLengthController[ctr].transform.localScale = curScale;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (scaleDirection == Vector3.up)
        {
            if (boosterLengthController[0].transform.localScale.y / yScale[0] < boosterPercentage[0])
            {
                booster_on();
            }
            if (boosterLengthController[0].transform.localScale.y / yScale[0] > boosterPercentage[0])
            {
                booster_off();
            }
        }
        if (scaleDirection == Vector3.forward)
        {
            if (Mathf.Abs(boosterLengthController[0].transform.localScale.z / yScale[0]) < boosterPercentage[0])
            {
                booster_on();
            }
            if (Mathf.Abs(boosterLengthController[0].transform.localScale.z / yScale[0]) > boosterPercentage[0])
            {
                booster_off();
            }
        }
    }
}
