using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
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
    void Start()
    {
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
        if (Vector3.Distance(transform.position, Player.transform.position) < 15) state = 2;

        // Move the spider off the ground
        transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
    }

    void Attack()
    {

    }

    void SurroundPlayer()
    {

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
        if (targetNumber != prevNumber)
        {
            Enemy.SetDestination(target);
        }
        // Set the prev target
        prevNumber = targetNumber;
    }
}
