using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum CollisionTypes
{
    NONE,
    ENVIRONMENT
}

public enum CharType
{
    NONE,
    PLAYER,
    ENEMY,
    ALLY
}

public class Character : MonoBehaviour
{

    public CharType myType = CharType.NONE;
    public string characterName;
    public AnimationClip deathAnimation;

    //AI variables
    protected GameObject player;
    protected Character playerScript;

    public int baseHp;
    public float baseArmor;
    public float baseDamage;

    protected Stats curStats; /*This keep's track of character's current stats*/
    protected Stats baseStats; /*All values within this keeps track of all the base stats
	                  *THIS SHOULD NEVER BE MODIFIED*/

    bool isUnitActive = true;

    //Variables shared by all character
    //**IDLE does not mean Character is not doing anything. Non idle means that the actor is doing something due to external commands**
    public string curCharacterState = "IDLE";
    /*Checks if message is received this frame.*/
    protected bool messageReceived = false;


    protected bool destroyReady = false;
    int enemyUnitIndex;

    protected BattleType curBattleType;

    //Misc animation clips
    public AnimationClip hitAnimation;


    //set target
    public GameObject target; /*Used to store the enemy that the player is currently looking at*/
    public Character targetScript;

    //Saves current targets
    public Character[] targets;
    public MapChargeFlag mapFlag;
    public float chargeStrength;


    //Detonator variable
    public GameObject detonatorFlinch;
    public GameObject detonatorFlinchMelee;
    public GameObject detonatorDeath;

    //Set boundary of movement
    public Collider battleBoundary;

    public float energyPercentage;
    public float maxEnergy;
    public float curEnergy;
    public float rotSpeed3D = 270.0f;
    public float rotSpeed = 180.0f;

    float volumeMassConversionCoeff;
    float massOfUnit;

    public Debuff characterDebuffScript;

    bool unitCollision;
    public bool isNonPlayer = true;

    GameObject characterFacing;
    public DropShip dropShipScript;
    public AnimationClip dropAnimation;

    protected Vector3 movement = Vector3.zero;
    public bool modifyPath = false;

    protected bool enemyReady = false;
    public bool landCraftActive;
    public GameObject unitIndicatorRing;

    Vector3 aiPreviousPos;
    public int startingChildCount;
    protected bool objectHit = false;


    protected int initLevel;

    CollisionTypes collisionStatus;

    public bool can_receive_detonator()
    {
        if (transform.childCount < startingChildCount + 3)
        {
            return true;
        }
        return false;
    }

    public void set_level(int level)
    {
        initLevel = level;
    }

    bool deathPhasedPlayed = false;

    public bool is_ready()
    {
        return enemyReady;
    }

    public void unit_successfully_landed()
    {
        if (unitIndicatorRing != null)
            unitIndicatorRing.SetActive(true);

        enemyReady = true;
    }




    /*
     **Important**
    All animations/actions should be handled when it receives message from the
    event control center. 
     */
    public void set_battle_type(BattleType input)
    {
        curBattleType = input;
    }

    Vector3 get_xz_component(Vector3 input)
    {
        Vector3 ret = new Vector3(input.x, 0.0f, input.z);
        return ret;
    }

    Vector3 get_yz_component(Vector3 input)
    {
        Vector3 ret = new Vector3(0.0f, input.y, input.z);
        return ret;
    }

    public bool custom_look_at_3D(Vector3 inPosition)
    {
        Vector3 targetPosition = inPosition;
        Vector3 playerPos = collider.bounds.center;
        float rotAngleY = Vector3.Angle(get_xz_component(transform.forward), get_xz_component(targetPosition - transform.position));
        float rotAngleX = Vector3.Angle(Vector3.forward, get_yz_component(transform.InverseTransformPoint(targetPosition)));


        /*
        if (transform.InverseTransformPoint(targetPosition).z < 0)
        {
            rotAngleX = 0.0f;
        }
        */
        float xRotationValue = rotSpeed3D * (rotAngleX / (rotAngleX + rotAngleY));
        float yRotationValue = rotSpeed3D * (rotAngleY / (rotAngleX + rotAngleY));


        float rotDirectionY = transform.InverseTransformPoint(targetPosition).x;
        float rotDirectionX = transform.InverseTransformPoint(targetPosition).y;

        if (Mathf.Abs(rotAngleY) > 0.0f)
        {
            if (rotAngleY > rotSpeed3D * Time.deltaTime)
            {
                if (rotDirectionY > 0)
                {
                    transform.Rotate(Vector3.up, yRotationValue * Time.deltaTime, Space.World);
                }
                else if (rotDirectionY < 0)
                {
                    transform.Rotate(Vector3.down, yRotationValue * Time.deltaTime, Space.World);
                }
            }
            else
            {

                if (rotDirectionY > 0)
                    transform.Rotate(Vector3.up, rotAngleY, Space.World);
                else if (rotDirectionY < 0)
                    transform.Rotate(Vector3.down, rotAngleY, Space.World);
            }
        }
        if (Mathf.Abs(rotAngleX) > 0.0f && Mathf.Abs(rotAngleY) <= 90.0f)
        {
            if (rotAngleX > rotSpeed3D * Time.deltaTime)
            {
                //Debug.Log("X axis Rotation Rate: " + rotSpeed * Time.deltaTime);
                if (rotDirectionX > 0)
                {
                    transform.Rotate(Vector3.left, xRotationValue * Time.deltaTime/*, Space.World*/);
                }
                else if (rotDirectionX < 0)
                {
                    transform.Rotate(Vector3.right, xRotationValue * Time.deltaTime/*, Space.World*/);
                }
            }
            else
            {
                if (rotDirectionX > 0)
                    transform.Rotate(Vector3.left, rotAngleX/*, Space.World*/);
                else if (rotDirectionX < 0)
                    transform.Rotate(Vector3.right, rotAngleX/*, Space.World*/);
            }
        }

        return true;
    }


    protected bool custom_look_at(Vector3 position)
    {
        float rotAngle = Vector3.Angle(transform.forward, position - transform.position);

        if (Mathf.Abs(rotAngle) < 0.04)
        {
            return false;
        }
        float rotDirection = transform.InverseTransformPoint(position).x;
        if (rotAngle > rotSpeed * Time.deltaTime)
        {
            if (rotDirection > 0)
            {
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            }
            else if (rotDirection < 0)
            {
                transform.Rotate(Vector3.up * -rotSpeed * Time.deltaTime);
            }
        }
        else
        {

            if (rotDirection > 0)
                transform.Rotate(Vector3.up * rotAngle);
            else if (rotDirection < 0)
                transform.Rotate(Vector3.down * rotAngle);

            //transform.LookAt(target.transform.position);
        }
        return true;
    }

    public void set_enemy_unit_index(int indexInput)
    {
        enemyUnitIndex = indexInput;
    }

    public int get_enemy_index()
    {
        return enemyUnitIndex;
    }

    //Obsolete
    public GameObject get_currently_facing()
    {
        return characterFacing;
    }

    public void set_player(GameObject inPlayer)
    {
        player = inPlayer;
        target = player;
        playerScript = player.GetComponent<Character>();
    }

    public void set_target(GameObject inTarget)
    {
        target = inTarget;
        targetScript = inTarget.GetComponent<Character>();
    }

    public Stats return_cur_stats()
    {
        return curStats;
    }

    public Stats return_base_stats()
    {
        return baseStats;
    }


    Vector3 flag_charge_force(MapChargeFlag flagCharges)
    {
        Vector3 retVector = Vector3.zero;
        retVector = flagCharges.transform.position - transform.position;
        float chargeForce = flagCharges.charge - retVector.magnitude;
        /*
        if (battleBoundary.bounds.extents.x > battleBoundary.bounds.extents.z)
        {
            chargeForce = flagCharges.charge * retVector.magnitude / (battleBoundary.bounds.extents.z * 0.5f);
        }
        else
        {
            chargeForce = flagCharges.charge * retVector.magnitude / (battleBoundary.bounds.extents.x * 0.5f);
        }
         * */
        if (flagCharges.suctionField > 0)
            chargeForce = flagCharges.charge * retVector.magnitude / (flagCharges.suctionField);
        if (flagCharges.suctionField <= 0)
        {
            chargeForce = flagCharges.charge * retVector.magnitude / (10.0f);
        }
        retVector *= chargeForce;
        //Debug.Log("Flag charge force: " + flagCharges.charge);
        if (chargeForce > 0.0f)
        {
            //Debug.Log("Positive charge");
            //Debug.DrawRay(transform.position, retVector, Color.blue);
        }
        if (chargeForce < 0.0f)
        {
            //Debug.Log("Negative charge");
            //Debug.DrawRay(transform.position, retVector, Color.yellow);
        }
        return retVector;
    }

    Vector3 character_charge_force(Character objectCharge)
    {
        Vector3 retVector = Vector3.zero;
        retVector = transform.position - objectCharge.transform.position;
        float chargeForce = objectCharge.chargeStrength - retVector.magnitude * 1.0f;
        retVector *= chargeForce;
        if (chargeForce < 0.0f)
        {
            chargeForce = 0.0f;
        }
        else
        {
            //Debug.DrawRay(transform.position, retVector, Color.red);
        }
        return retVector;
    }

    //In global coordinates
    public Vector3 find_movement_vector()
    {
        Vector3 retVector = Vector3.zero;
        foreach (Character objectCharge in targets)
        {
            if (objectCharge != null)
                retVector += character_charge_force(objectCharge);
        }
        if (mapFlag != null)
            retVector += flag_charge_force(mapFlag);
        if (retVector != Vector3.zero)
        {
            //Debug.DrawRay(transform.position, transform.forward * 10.0f, Color.black);
            //Debug.DrawRay(transform.position, retVector.normalized * 10.0f, Color.green);
        }
        if (retVector.magnitude < 2.0f)
        {

            return Vector3.zero;
        }

        return retVector.normalized;
    }


    /*Check line of sight returns gameObject the object is currently looking at.
     If object is not looking at any object with a collider, returns false.*/
    public GameObject check_line_of_sight()
    {
        Ray ray = new Ray();
        ray.direction = (target.collider.bounds.center - collider.bounds.center).normalized;
        ray.origin = collider.bounds.center;
        GameObject nullObject = null;
        characterFacing = null;
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000.0f))
        {
            if (hitData.collider.tag == "Character")
            {
                characterFacing = hitData.collider.gameObject;
                return hitData.collider.gameObject;
            }
            else
            {
                //Debug.Log("Looking at target " + hitData.collider.gameObject);
            }
        }
        return nullObject;
    }

    public void calculate_object_mass()
    {
        Collider colliderBox = this.GetComponent<Collider>();
        massOfUnit = volumeMassConversionCoeff * colliderBox.bounds.size.x * colliderBox.bounds.size.y
            * colliderBox.bounds.size.z;
    }


    /*Returns true if movement has been completely made. Else no movement
     is made and returns false*/
    public bool custom_translate(Vector3 inputMovement)
    {
        Vector3 curPosition = transform.position;
        curPosition += 10 * inputMovement;
        //if (battleBoundary.bounds.Contains(curPosition))
        //{
        transform.Translate(inputMovement);

        return true;
        //}
        //return false;
    }


    //Checks and resolves any issues where enemy goes out of the battle scene
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Character")
        {
            unitCollision = true;
        }
        if (hit.collider.tag == "Environment" && isNonPlayer == true)
        {
            collisionStatus = CollisionTypes.ENVIRONMENT;
            transform.position = aiPreviousPos;
        }
    }


    /*
	void collision_resolution () {

	}
    */


    /*The following functions below are used to modify any damage 
     calculations. Used by players or AI to modify as damages are applied*/

    /*This function handles the character being hit. It should calculate the
     actual damage that the character receives based on the raw damage value and 
     apply it to the character. Then it should return the applied damage. By
     default, rawDamage is applied directly*/
    public float hit(float rawDamage)
    {
        if (isUnitActive == true)
        {
            if (rawDamage >= 0)
            {
                //curCharacterState = "HIT";
                messageReceived = true;
                float damageDone = rawDamage;
                if (curStats.armor < 90.0f && curStats.armor > 0.0f)
                {
                    damageDone = (1.0f * rawDamage * (1.0f - 1.0f * baseStats.armor / 100.0f));
                }
                if (curStats.armor >= 90.0f)
                {
                    damageDone = (1.0f * rawDamage * (0.1f));
                }
                if (curStats.armor <= 0.0f)
                {
                    damageDone = rawDamage;
                }
                curStats.hp -= (int)damageDone;

                if (detonatorFlinch != null /*&& transform.childCount < startingChildCount + 5*/)
                {
                    if (detonatorFlinchMelee == null)
                    {
                        GameObject temp = (GameObject)Instantiate(detonatorFlinch, collider.bounds.center, Quaternion.identity);
                        temp.transform.parent = gameObject.transform;
                    }
                    else
                    {
                        GameObject temp = (GameObject)Instantiate(detonatorFlinchMelee, collider.bounds.center, Quaternion.identity);
                        temp.transform.parent = gameObject.transform;
                    }
                }

                if (hitAnimation != null)
                {
                    animation.Play(hitAnimation.name);
                }
                if (curStats.hp < 0.0f)
                    collider.enabled = false;

                objectHit = true;
                return damageDone;
            }
            else
            {
                curStats.hp -= (int)rawDamage;
                if (curStats.hp > baseStats.hp)
                {
                    curStats.hp = baseStats.hp;
                }
                return rawDamage;
            }
        }
        return 0.0f;
    }

    public float hit(float rawDamage, Vector3 hitPosition)
    {
        if (isUnitActive == true)
        {
            if (rawDamage >= 0)
            {
                //curCharacterState = "HIT";
                messageReceived = true;
                float damageDone = rawDamage;
                if (curStats.armor < 90.0f && curStats.armor > 0.0f)
                {
                    damageDone = (1.0f * rawDamage * (1.0f - 1.0f * baseStats.armor / 100.0f));
                }
                if (curStats.armor >= 90.0f)
                {
                    damageDone = (1.0f * rawDamage * (0.1f));
                }
                if (curStats.armor <= 0.0f)
                {
                    damageDone = rawDamage;
                }
                curStats.hp -= (int)damageDone;

                if (detonatorFlinch != null)
                    //Instantiate(detonatorFlinch, hitPosition, Quaternion.identity);

                    if (hitAnimation != null)
                    {
                        animation.Play(hitAnimation.name);
                    }
                if (curStats.hp < 0.0f)
                    collider.enabled = false;
                return damageDone;
            }
            else
            {
                curStats.hp -= (int)rawDamage;
                if (curStats.hp > baseStats.hp)
                {
                    curStats.hp = baseStats.hp;
                }
                return rawDamage;
            }
        }
        return 0.0f;
    }

    protected void death_state()
    {
        if (deathPhasedPlayed == false)
        {
            deathPhasedPlayed = true;
            if (deathAnimation != null)
                animation.Play(deathAnimation.name);
            else
                Destroy(gameObject);
        }
        else
        {
            if (!animation.IsPlaying(deathAnimation.name))
            {
                if (detonatorDeath)
                    Instantiate(detonatorDeath, collider.bounds.center, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }


    /*This function handles the restored value of character's health. It should
     take the heal value and add it to the hp pool of this character. If heal type
     is 0, apply 100% value of the heal value. If it is any other value, apply the
     percentage that is determined by heal type with heal value to the character
     hp pool. If specified heal type does not exist for the character, use
     value of 0.*/
    public void regen_health(float healValue)
    {
        curStats.hp += (int)healValue;
    }

    public void modify_stat(float armorMod, float attackMod)
    {
        curStats.armor += (int)armorMod;
        curStats.damage += (int)attackMod;

        Debug.Log("My Armor: " + curStats.armor);
    }

    /*Returns true when object is ready to be destroyed*/
    public bool is_destroy_ready()
    {
        return destroyReady;
    }

    public virtual void first_wave()
    {
    }

    /*Use this update! DO NOT USE Update() function!
     use this function instead.*/
    public virtual void manual_start()
    {
        startingChildCount = transform.childCount;
        characterDebuffScript = this.GetComponent<Debuff>();
        if (characterDebuffScript == null)
        {
            Debug.LogError("Debuff script not found");
        }
        else
        {
            characterDebuffScript.initialize_script();
        }
        if (dropShipScript != null)
        {
            if (landCraftActive == true)
            {
                dropShipScript.position_in_the_air();
            }
            else
            {
                unit_successfully_landed();
                Destroy(dropShipScript.gameObject);
            }
            if (dropAnimation != null)
                animation.Play(dropAnimation.name);
        }
        else
        {
            enemyReady = true;
        }
        if (isNonPlayer == true)
        {
            AIStatElement aiStat = GetComponent<AIStatScript>().getLevelData(initLevel);
            curStats.armor = (int)aiStat.baseArmor;
            curStats.hp = aiStat.hp;
            curStats.damage = (int)aiStat.baseAttack;
            baseStats = curStats;
        }

    }

    public virtual void manual_update()
    {
    }

    public virtual void precombat_phase()
    {

    }

    public virtual void LateUpdate()
    {
        aiPreviousPos = transform.position;
    }
}
