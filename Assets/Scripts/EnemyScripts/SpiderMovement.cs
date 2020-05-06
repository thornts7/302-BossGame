using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public List<GameObject> MovePoints = new List<GameObject>();
    NavMeshAgent Enemy;
    Vector3 StartPos;

    void Start()
    {
        Enemy = gameObject.GetComponent<NavMeshAgent>();
        Enemy.SetDestination(MovePoints[1].transform.position);
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
    }
}
