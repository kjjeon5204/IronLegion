﻿using UnityEngine;
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

[System.Serializable]
public struct BaseBattleStat
{
    public int hp;
    public int armor;
    public int damage;
    public int penetration;
}

public class Character : MonoBehaviour
{
    

    public CharType myType = CharType.NONE;
    public string characterName;
    public AnimationClip deathAnimation;

    
    //These two variables store base stat and current stat of the enemy throughout the duration of the 
    //battle. Base stat is set at initilization of the AI and should never be modified. Current stat
    //is modified throughout the duration of the battle.
    protected BaseBattleStat baseStats;
    protected BaseBattleStat curStats;

    public BaseBattleStat get_cur_stats()
    {
        return curStats;
    }

    bool isUnitActive = true;

    //Variables shared by all character
    //**IDLE does not mean Character is not doing anything. Non idle means that the actor is doing something due to external commands**
    public int curCharacterState;
    /*Checks if message is received this frame.*/
    protected bool messageReceived = false;


    protected bool destroyReady = false;
    int enemyUnitIndex;

    protected BattleType curBattleType;

    //Misc animation clips
    public AnimationClip hitAnimation;


    //set target
    IList<Character> enemyCharacter;
    IList<Character> allyCharacter;

    protected Character target;
    public Character get_target() 
    {
        return target;
    }

    //Pathing & positioning
    public MapChargeFlag mapFlag;
    public float chargeStrength;

    //Detonator variable
    public GameObject detonatorDeath;
    public Queue<GameObject> detonatorQueue;
    int detonatorQueueLimit = 4;

    //Set boundary of movement
    public float rotSpeed3D = 270.0f;
    public float rotSpeed = 180.0f;

    public Debuff characterDebuffScript;

    bool unitCollision;

    //Enemy Land unit spawn
    public DropShip dropShipScript;
    public AnimationClip dropAnimation;
    protected bool enemyReady = false;
    public bool landCraftActive;
    public GameObject unitIndicatorRing;

    //Player pathing
    protected Vector3 movement = Vector3.zero;
    public bool modifyPath = false;


    Vector3 aiPreviousPos;
    public int startingChildCount;
    protected bool objectHit = false;


    protected int initLevel;

    CollisionTypes collisionStatus;
    protected BaseCombatStructure baseCombatStructure;

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


    public bool custom_look_at(Vector3 position)
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


    public void set_player(GameObject inPlayer)
    {
        /*
        player = inPlayer;
        target = player;
        playerScript = player.GetComponent<Character>();
         */ 
    }

    public void set_target(GameObject inTarget)
    {
        /*
        target = inTarget;
        targetScript = inTarget.GetComponent<Character>();
         */ 
    }

    public Stats return_cur_stats()
    {
        //return curStats;
        Stats temp = new Stats();
        return temp;
    }

    public Stats return_base_stats()
    {
        //return baseStats;
        Stats temp = new Stats();
        return temp;
    }

    public void initialize_character(BaseCombatStructure baseCombatStructureInput)
    {
        baseCombatStructure = baseCombatStructureInput;
    }

    public void initialize_character(BaseCombatStructure baseCombatStructureInput, 
        MapChargeFlag inputChargeFlag)
    {
        baseCombatStructure = baseCombatStructureInput;
        mapFlag = inputChargeFlag;
    }

    public void initialize_character(BaseCombatStructure baseCombatStructureInput,
        MapChargeFlag inputChargeFlag, int inputAccNum)
    {
        baseCombatStructure = baseCombatStructureInput;
        mapFlag = inputChargeFlag;
        enemyUnitIndex = inputAccNum;
    }


    Vector3 flag_charge_force(MapChargeFlag flagCharges)
    {
        Vector3 retVector = Vector3.zero;
        retVector = flagCharges.transform.position - transform.position;
        float chargeForce = flagCharges.charge - retVector.magnitude;
        if (flagCharges.suctionField > 0)
            chargeForce = flagCharges.charge * retVector.magnitude / (flagCharges.suctionField);
        if (flagCharges.suctionField <= 0)
        {
            chargeForce = flagCharges.charge * retVector.magnitude / (10.0f);
        }
        retVector *= chargeForce;
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
        return retVector;
    }

    //In global coordinates
    public Vector3 find_movement_vector()
    {
        
        Vector3 retVector = Vector3.zero;
        /*
        foreach (Character objectCharge in targets)
        {
            if (objectCharge != null)
                retVector += character_charge_force(objectCharge);
        }
         */ 
        if (mapFlag != null)
            retVector += flag_charge_force(mapFlag);

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
        //ray.direction = (target.collider.bounds.center - collider.bounds.center).normalized;
        ray.origin = collider.bounds.center;
        GameObject nullObject = null;
        //characterFacing = null;
        //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 1000.0f))
        {
            if (hitData.collider.tag == "Character")
            {
                //characterFacing = hitData.collider.gameObject;
                return hitData.collider.gameObject;
            }
            else
            {
                //Debug.Log("Looking at target " + hitData.collider.gameObject);
            }
        }
        return nullObject;
    }



    /*Returns true if movement has been completely made. Else no movement
     is made and returns false*/
    public bool custom_translate(Vector3 inputMovement)
    {
        Vector3 curPosition = transform.position;
        curPosition += 10 * inputMovement;
        transform.Translate(inputMovement);

        return true;
    }


    //Checks and resolves any issues where enemy goes out of the battle scene
    void OnTriggerEnter(Collider hit)
    {
        /*
        if (hit.gameObject.tag == "Character")
        {
            unitCollision = true;
        }
        if (hit.collider.tag == "Environment" && isNonPlayer == true)
        {
            collisionStatus = CollisionTypes.ENVIRONMENT;
            transform.position = aiPreviousPos;
        }
         */ 
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
        /*
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

                if (detonatorFlinch != null /*&& transform.childCount < startingChildCount + 5)
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
         */ 
        return 0.0f;
    }

    public float hit(float rawDamage, Vector3 hitPosition)
    {
        /*
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
         */ 
        return 0.0f;
    }

    public virtual void death_state()
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
        /*
        curStats.hp += (int)healValue;
         */ 
    }

    public void modify_stat(float armorMod, float attackMod)
    {
        /*
        curStats.armor += (int)armorMod;
        curStats.damage += (int)attackMod;

        Debug.Log("My Armor: " + curStats.armor);
         */ 
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
        /*
        if (isNonPlayer == true)
        {
            AIStatElement aiStat = GetComponent<AIStatScript>().getLevelData(initLevel);
            curStats.armor = (int)aiStat.baseArmor;
            curStats.hp = aiStat.hp;
            curStats.damage = (int)aiStat.baseAttack;
            baseStats = curStats;
        }
         */ 

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
