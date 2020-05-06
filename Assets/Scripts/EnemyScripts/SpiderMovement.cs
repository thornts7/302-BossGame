using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public List<GameObject> MovePoints = new List<GameObject>();
    NavMeshAgent Enemy;
    int targetNumber = 1;
    int prevNumber;

    Vector3 StartPos;
    Vector3 target;

    float percent = 0;
    Quaternion StartRotation;
    Quaternion EndRotation;

    void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        target = MovePoints[targetNumber].transform.position;
        Enemy.SetDestination(target);
        Enemy.updateRotation = false;
        
        StartPos = transform.position;

        StartRotation = transform.rotation;
        Vector3 StartEular = transform.rotation.eulerAngles;
        EndRotation = Quaternion.LookRotation(target);
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
            StartRotation = transform.rotation;
            Vector3 StartEular = transform.rotation.eulerAngles;
            EndRotation = Quaternion.LookRotation(MovePoints[targetNumber].transform.position);
            percent = 0;
        } else if (Vector3.Distance(transform.position, target) >= 3)
        {
            if (percent < 1) percent += 0.01f;

            transform.rotation = Quaternion.Slerp(StartRotation, EndRotation, percent);
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
