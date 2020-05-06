using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public List<GameObject> MovePoints = new List<GameObject>();
    NavMeshAgent Enemy;
    Vector3 target;
    int targetNumber = 1;
    int prevNumber;
    float Rotation = 0;

    Vector3 StartPos;

    void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        target = MovePoints[targetNumber].transform.position;
        Enemy.SetDestination(target);
        Enemy.updateRotation = false;
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 3)
        {
            int newTarget = Random.Range(0, 3);
            while (newTarget == targetNumber)
            {
                newTarget = Random.Range(0, 3);
            }
            targetNumber = newTarget;
        } else if (Vector3.Distance(transform.position, target) >= 3)
        {

        }


        if (targetNumber != prevNumber)
        {
            target = MovePoints[targetNumber].transform.position;
            Enemy.SetDestination(target);
        }
        transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
        prevNumber = targetNumber;
    }
}
