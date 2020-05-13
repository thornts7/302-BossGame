using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public delegate void EnemyStuff();
    public static EnemyStuff EnemyHurt;

    // The possible points to move to
    public List<GameObject> MovePoints = new List<GameObject>();
    // References NavMeshAgent
    NavMeshAgent Enemy;
    // The target mesh number
    int targetNumber = 1;
    // the target mesh number from thw previous frame
    int prevNumber;

    // The starting position
    Vector3 StartPos;
    // Target's position
    Vector3 target;

    // The quaternion representing the desired angle
    Quaternion desiredRotation;

    // The current state of the Enemy
    int state = 1;

    // The refernence the the player
    GameObject Player;
    Vector3 PrevPlayerPos;

    // The midrange target
    Vector3 MidRangeTarget;
    Vector3 MoveOffset;
    // The counters that allow the Spider to shooty shooty
    float ShootCounter = 0;
    float AttackCounter = 0;
    // The number that determins the offset in surround player mode
    int Otype;
    int PrevOtype;

    [Header("Gun Stuff")]
    // The Turret on top
    public Transform Gun;
    // The point the gun shoots out of
    public Transform ShootPoint;
    // The rotaton before aiming at player
    Quaternion OriginalGunRotation;
    // The bullet prefab
    public GameObject Bullet;
    // The counter that queues the cacheing of the rotation
    float StashCounter = 0;

    ////////////////////////////// Gameplay
    int Health = 100;

    void Start()
    {
        EnemyHurt += enemyIsDamaged;

        // Get the player's Position
        Player = GameObject.Find("character1");
        
        // The NavMesh Agent
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        // Randomly setting the target
        targetNumber = Random.Range(0, 3);
        // Setting the Vector target
        target = MovePoints[targetNumber].transform.position;
        // Setting the agent's destination
        Enemy.SetDestination(target);
        // Sets the rotation to not update automatically
        Enemy.updateRotation = false;
        
        // Setting the starting position so I can set the the y location every frame
        StartPos = transform.position;

        // This is the vector we plan to move towared
        Vector3 targetVector = target - transform.position;
        // This is the quaternion version of the rotation
        desiredRotation = Quaternion.LookRotation(targetVector);
    }

    void Update()
    {
        switch (state) // Makeshift state machine
        {
            case 1:
                Wander();
                break;
            case 2:
                SurroundPlayer();
                break;
            case 3:
                Attack();
                break;
        }
        // State changing when in and out of range
        if (Vector3.Distance(transform.position, Player.transform.position) > 40) state = 1;
        if (Vector3.Distance(transform.position, Player.transform.position) < 15 && state == 1) state = 2;

        Debug.DrawLine(Player.transform.position, MidRangeTarget, Color.red);

        // Move the spider off the ground
        transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
        PrevPlayerPos = Player.transform.position;
    }

    void Attack()
    {
        // Stashing Original Rotation To Set It Back
        if (StashCounter == 0)
        {
            OriginalGunRotation = Gun.rotation;
            StashCounter = 1;
        }
        // Aim at player
        Vector3 targetVector = Player.transform.position - Gun.position;
        Quaternion DRot = Quaternion.LookRotation(targetVector) * Quaternion.Euler(-90, 180, 0);
        Gun.rotation = Quaternion.RotateTowards(Gun.rotation, DRot, 90 * Time.deltaTime);

        // Counter tick
        ShootCounter += Time.deltaTime;
        // Shoot and Change state
        if (ShootCounter > 2)
        {
            Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
            ShootCounter = 0;
            StashCounter = 0;
            Gun.rotation = OriginalGunRotation;
            state = 2;
        }
    }

    void SurroundPlayer()
    {
        //////////////////////////////////////////// Movement
        ///
        if ((Vector3.Distance(transform.position, MidRangeTarget) < 2) || (Vector3.Distance(transform.position, MidRangeTarget + MoveOffset) < 2))
        {
            Otype = Random.Range(1,4);
            while (Otype == PrevOtype)
            {
                Otype = Random.Range(1, 4);
            }
            if (Otype == 1) MoveOffset = new Vector3(3, 0, 3);
            if (Otype == 2) MoveOffset = new Vector3(-3, 0, 3);
            if (Otype == 3) MoveOffset = new Vector3(3, 0, -3);
            if (Otype == 4) MoveOffset = new Vector3(-3, 0, -3);
        }
        if (Player.transform.position != PrevPlayerPos)
        {
            // Update location of Spider Move Target
            float x = transform.position.x - Player.transform.position.x;
            float z = transform.position.z - Player.transform.position.z;
            float angleToPlayer = Mathf.Atan2(x, z);
            MidRangeTarget.x = 15 * Mathf.Sin(angleToPlayer) + Player.transform.position.x;
            MidRangeTarget.z = 15 * Mathf.Cos(angleToPlayer) + Player.transform.position.z;
            MidRangeTarget.y = Player.transform.position.y;
        }

        Vector3 MRTWO = MidRangeTarget + MoveOffset;
        
        // Change target to Spider Move Target
        Enemy.SetDestination(MRTWO);
        
        /////////////////////////////////////////// Rotation
        // Update rotation vector
        Vector3 targetVector = Player.transform.position - transform.position;
        // Get quaternion of rotation
        Quaternion DRot = Quaternion.LookRotation(targetVector);
        // Update rotation of the spider
        transform.rotation = Quaternion.RotateTowards(transform.rotation, DRot, 90 * Time.deltaTime);
        
        //////////////////////////////////////////// State changing
        // Change state of Spider when: IsAttacking
        if (AttackCounter > 3)
        {
            state = 3;
            AttackCounter = 0;
        }
        // Counter tick
        AttackCounter += Time.deltaTime;
        PrevOtype = Otype;
    }

    /// <summary>
    /// This is the wander State
    /// The boss walks around to the designated points around the map
    /// </summary>
    void Wander()
    {
        if (Vector3.Distance(transform.position, target) < 3)
        {
            // Calculates a new target to go to
            int newTarget = Random.Range(0, 3);
            // Checks if the number is the same as the current one
            while (newTarget == targetNumber)
            {
                newTarget = Random.Range(0, 3);
            }
            // Set the new target
            targetNumber = newTarget;
            // Sets the vector3 target to the location of that game object
            target = MovePoints[targetNumber].transform.position;
            // This is the vector we will point towards
            Vector3 targetVector = target - transform.position;
            // this is the quaternion that represents the desired rotation
            desiredRotation = Quaternion.LookRotation(targetVector);
        }
        else if (Vector3.Distance(transform.position, target) >= 3)
        {
            // This is the change in rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, 90 * Time.deltaTime);
        }

        // if the target is different change the agent's target
        if (targetNumber != prevNumber || Enemy.destination != target)
        {
            Enemy.SetDestination(target);
        }
        // Set the prev target
        prevNumber = targetNumber;
    }
    void enemyIsDamaged()
    {
        Health--;
    }
}
