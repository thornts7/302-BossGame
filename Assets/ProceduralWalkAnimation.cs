using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalkAnimation : MonoBehaviour
{
    Vector3 startingPosition;
    float magnitude = 2;
    void Start()
    {
        startingPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        float offsetZ = Mathf.Sin(Time.time)/magnitude;
        float offestY = Mathf.Cos(Time.time)/magnitude;
        if (offestY < 0) offestY = 0;

        Vector3 finalPosition = startingPosition;

        finalPosition.y += offestY + .01f; // move final position up or down
        finalPosition.z += offsetZ; // move forward and backward

        transform.localPosition = finalPosition;
    }
}
