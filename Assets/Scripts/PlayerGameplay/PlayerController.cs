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

    public List<GameObject> Shootpoints = new List<GameObject>();
    public GameObject Projectile;
    public List<GameObject> WristConstraints = new List<GameObject>();
    List<Vector3> WCOriginalPos = new List<Vector3>();
    public GameObject Camera;
    int alternator = 0;
    bool HasShot;

    int health = 50;
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

        ////////////////////////////////////// Combat

        // Point hands where camera is pointed
        // Rotate (try for this)
        WristConstraints[0].transform.rotation = Camera.transform.rotation * Quaternion.Euler(-90, 0, 0);
        WristConstraints[1].transform.rotation = Camera.transform.rotation * Quaternion.Euler(-90, 0, 0);
        // Move

        // Create bullet at one of the two shoot points on mouse click
        if (Input.GetMouseButtonDown(0) && !HasShot)
        {
            Instantiate(Projectile, Shootpoints[alternator].transform.position, Shootpoints[alternator].transform.rotation);
            alternator = (alternator == 0) ? 1 : 0;
            HasShot = true;
        } else if (Input.GetMouseButtonDown(0) && HasShot)
        {
            print("ok");
        } else
        {
            HasShot = false;
        }
        // Make hands animated
        // Make hands move back and then forth

        // Make hands move up and then down

        // Make head follow camera as well
        // Simple rotational lock with clamped angles
    }
}
