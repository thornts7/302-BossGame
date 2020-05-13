using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    float counter = 0;
    void Start() { GetComponent<Rigidbody>().velocity = transform.up * 40; }
    private void Update() { counter++; if (counter > 120) Destroy(gameObject); }
    private void OnCollisionEnter(Collision collision) { Destroy(gameObject); }
}
