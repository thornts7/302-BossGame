using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement2 : MonoBehaviour
{
    float counter = 0;
    void Start() { GetComponent<Rigidbody>().velocity = transform.right * 10; }
    private void Update() { counter++; if (counter > 600) Destroy(gameObject); }
    private void OnCollisionEnter(Collision collision) { /*Destroy(gameObject);*/ }
}
