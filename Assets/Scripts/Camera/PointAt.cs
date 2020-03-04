using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{

    public Transform target;

    void Update()
    {
        if (target == null) return;

        Vector3 look = target.position - transform.position;
        look.Normalize();

        transform.rotation = Quaternion.LookRotation(look, Vector3.up);
    }
}
