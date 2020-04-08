using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalkAnimation : MonoBehaviour
{
    Vector3 startingPosition;
    public Transform HintPos;
    Vector3 hintStartingPos;
    float magnitude = 2;


    public float sinWaveSpeed;
    public float sinWaveOffset;

    float feetDistancing = 1.5f;
    float footHeight = .6f;
    float howFarFeetGo = .5f;
    PlayerController player;

    void Start()
    {
        startingPosition = transform.localPosition;
        hintStartingPos = HintPos.localPosition;
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = (Time.time + sinWaveOffset * Mathf.PI) * sinWaveSpeed;
        float offsetZ = Mathf.Cos(time)/magnitude;
        float offestY = Mathf.Cos(time)/magnitude;
        if (offestY < 0.1f) offestY = 0.1f;
        
        Vector3 finalPosition = startingPosition;
        Vector3 finalHintPosition = hintStartingPos;

        finalPosition.x *= feetDistancing;
        finalPosition.y += offestY * footHeight; // move final position up or down

        Vector3 walkDir = transform.InverseTransformDirection(player.walkDir);
        float Y = walkDir.y;
        float Z = walkDir.z;
        walkDir.z = Y;
        walkDir.x = Z;
        walkDir.y = 0;

        finalPosition += walkDir * offsetZ * howFarFeetGo;
        finalHintPosition += walkDir * offsetZ;

        transform.localPosition = finalPosition;
    }
}
