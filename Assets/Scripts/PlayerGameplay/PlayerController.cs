using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PlayerStuff(GameObject bullet);
    public static PlayerStuff PlayerDamage;

    public float moveSpeed = 5;
    CharacterController pawn;
    float rotation = 180f;
    float inputSensitivity = 150;

    public Vector3 walkDir { get; private set; }

    public List<GameObject> Shootpoints = new List<GameObject>();
    public List<GameObject> WristConstraints = new List<GameObject>();
    List<Vector3> WCOriginalPos = new List<Vector3>();
    public GameObject Projectile;
    public GameObject Camera;
    public GameObject Neck;
    int alternator = 0;
    bool HasShot;

    public int health = 10;
    void Start()
    {
        PlayerDamage += HurtMe;
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
        float angle = Camera.transform.rotation.eulerAngles.x * Mathf.PI/180;
        float y = Mathf.Sin(angle);
        Vector3 B = WristConstraints[0].transform.position;
        Vector3 C = WristConstraints[2].transform.position;
        Vector3 D = WristConstraints[1].transform.position;
        WristConstraints[0].transform.position = new Vector3(B.x, C.y + y, B.z);
        WristConstraints[1].transform.position = new Vector3(D.x, C.y + y, D.z);

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
        // Simple rotational lock with clamped angles
        Vector3 A = Camera.transform.rotation.eulerAngles;
        //if (A.x < 20 && A.x < 90) A.x = 20;
        //else if (A.x < 300 && A.x > 260) A.x = 300;
        Neck.transform.rotation = Quaternion.Euler(A);
    }

    void HurtMe(GameObject bullet)
    {
        if (Vector3.Distance(bullet.transform.position, transform.position) < 3)
        {
            health--;
        }
    }
}
