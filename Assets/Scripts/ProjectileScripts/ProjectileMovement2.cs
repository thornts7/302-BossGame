using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement2 : MonoBehaviour
{
    float counter = 0;
    void Start() { GetComponent<Rigidbody>().velocity = transform.up * 20; }
    private void Update() { counter++; if (counter > 600) Destroy(gameObject); }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.name == "character1") collision.collider.gameObject.GetComponent<PlayerController>().health--;
        else
        {
            PlayerController.PlayerDamage(this.gameObject);
        }
        Destroy(gameObject);
    }
}
