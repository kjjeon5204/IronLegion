using UnityEngine;
using System.Collections;

public class AerialType2Fighter : Character {
    public float flightSpeed;
    public GameObject missileMuzzle;
    public GameObject missileProjectile;

    float startingHeightVal;

    float horizontalCamFulstrum;
    Vector3 rightApproachPoint;
    Vector3 leftApproachPoint;

    Vector3 rightPassPoint;
    Vector3 leftPassPoint;
    Vector3 rightPassPointWorld;
    Vector3 leftPassPointWorld;


    Vector3 rightMidPoint;
    Vector3 leftMidPoint;

    bool rightPassPointGate;
    bool rightApproachPointGate;

    float abortTimeTracker;

    public GameObject movePointIndicator;

    public enum FighterState
    {
        FLIGHT_PATH1,
        FLIGHT_PATH2,
        ATTACK,
        FLIGHT_PATH3
    }

    FighterState currentState;

    bool flightPathCalculated = false;

    public void calculate_flight_path()
    {
        float verticalCamFulstrum = Camera.main.fieldOfView;
        float distanceToNearPlane = 1.0f / Mathf.Tan(verticalCamFulstrum / 2.0f);
        horizontalCamFulstrum = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan
            (Camera.main.aspect / distanceToNearPlane));
        Debug.Log(horizontalCamFulstrum);
        rightApproachPoint = new Vector3(40.0f * Mathf.Cos(horizontalCamFulstrum), 0.0f, 40.0f);
        leftApproachPoint = new Vector3(-40.0f * Mathf.Cos(horizontalCamFulstrum), 0.0f, 40.0f);
        rightPassPoint = new Vector3(rightApproachPoint.x + 3.0f, 0.0f, -20.0f);
        leftPassPoint = new Vector3(leftApproachPoint.x - 3.0f, 0.0f, -20.0f);
    }

    public override void manual_start()
    {

        base.manual_start();
        startingHeightVal = transform.position.y;
        if (Camera.main != null)
        {
            calculate_flight_path();
            flightPathCalculated = true;
        }
        currentState = FighterState.FLIGHT_PATH1;
    }

    public override void manual_update()
    {
        if (flightPathCalculated == false)
        {
            calculate_flight_path();
            flightPathCalculated = true;
        }

        //base.manual_update();
        Vector3 curRelativePos = target.transform.InverseTransformPoint(transform.position);
        float relativeAngleToPlayer = Vector3.Angle(Vector3.forward, curRelativePos);
        transform.Translate(flightSpeed * Vector3.forward * Time.deltaTime);
        if (curRelativePos.x > 0.0f)
        {
            rightPassPointGate = false;
            rightApproachPointGate = true;
        }
        else
        {
            rightPassPointGate = true;
            rightApproachPointGate = false;
        }


        if (currentState == FighterState.FLIGHT_PATH1)
        {
            Vector3 approachPoint = Vector3.zero;
            if (rightApproachPointGate == true)
            {
                approachPoint = target.transform.TransformPoint(rightApproachPoint);
                approachPoint.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
                custom_look_at_3D(approachPoint);
            }
            else
            {
                approachPoint = target.transform.TransformPoint(leftApproachPoint);
                approachPoint.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
                custom_look_at_3D(approachPoint);
            }

            if (movePointIndicator != null)
                movePointIndicator.transform.position = approachPoint;

            //end condition
            float angleFromCam = Vector3.Angle(Vector3.forward, 
                Camera.main.transform.InverseTransformPoint(transform.position));

            

            if (angleFromCam < horizontalCamFulstrum && (transform.position - target.transform.position).magnitude > 30.0f)
            {
                currentState = FighterState.FLIGHT_PATH2;
                abortTimeTracker = Time.time + 3.0f;
            }
        }
        else if (currentState == FighterState.FLIGHT_PATH2)
        {
            custom_look_at_3D(target.transform.position);
            Vector3 targetRelativePos = transform.InverseTransformPoint(target.transform.position);

            if (movePointIndicator != null)
                movePointIndicator.transform.position = target.transform.position;

            if (Vector3.Angle(Vector3.forward, targetRelativePos) < 20.0f)
            {
                currentState = FighterState.ATTACK;
            }
            if (Time.time > abortTimeTracker)
            {
                currentState = FighterState.FLIGHT_PATH3;
                rightPassPointWorld = target.transform.position + 20.0f * transform.TransformDirection(1.0f, 0.0f, 1.0f);
                rightPassPointWorld.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
                leftPassPointWorld = target.transform.position + 20.0f * transform.TransformDirection(-1.0f, 0.0f, 1.0f);
                leftPassPointWorld.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
            }
        }
        else if (currentState == FighterState.ATTACK)
        {
            GameObject missileProjectileAccess = (GameObject)Instantiate(missileProjectile, missileMuzzle.transform.position, missileMuzzle.transform.rotation);
            missileProjectileAccess.GetComponent<MyProjectile>().set_projectile(target, gameObject, 5.0f);

            currentState = FighterState.FLIGHT_PATH3;
            //rightPassPointWorld = target.transform.TransformPoint(rightPassPoint);
            //leftPassPointWorld = target.transform.TransformPoint(leftPassPoint);

            
            rightPassPointWorld = target.transform.position + 20.0f * transform.TransformDirection(1.0f, 0.0f, 1.0f);
            rightPassPointWorld.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
            leftPassPointWorld = target.transform.position + 20.0f * transform.TransformDirection(-1.0f, 0.0f, 1.0f);
            leftPassPointWorld.y = startingHeightVal + Random.Range(-5.0f, 5.0f);
        }
        else if (currentState == FighterState.FLIGHT_PATH3)
        {
            Vector3 passPoint;
            if (rightPassPointGate == true) {
                custom_look_at_3D(rightPassPointWorld);
                passPoint = rightPassPointWorld;
            }
            else {
                custom_look_at_3D(leftPassPointWorld);
                passPoint = leftPassPointWorld;
            }


            if (movePointIndicator != null)
                movePointIndicator.transform.position = passPoint;

            if ((transform.position - passPoint).magnitude < 5.0f)
            {
                Debug.Log("Phase 1");
                currentState = FighterState.FLIGHT_PATH1;
            }
        }

    }
}
