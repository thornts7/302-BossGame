using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    CharacterController pawn;
    float rotation = 180f;
    float inputSensitivity = 150;

    public Vector3 walkDir { get; private set; }

    void Start()
    {
        pawn = GetComponent<CharacterController>();
        Vector3 rot = transform.rotation.eulerAngles;
        rotation = rot.y;
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////// Rotation
        float mouseX = Input.GetAxis("Mouse X");

        rotation += mouseX * inputSensitivity * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(0, rotation, 0.0f);
        transform.rotation = localRotation;

        ////////////////////////////////////// Movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        if (input.sqrMagnitude > 1) input.Normalize();

        walkDir = input.x * transform.right + input.z * transform.forward;

        pawn.SimpleMove(walkDir * moveSpeed);
    }
}
