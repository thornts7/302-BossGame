using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FootRaycast : MonoBehaviour
{
    public Transform ground;
    Vector3 raycastOffset =  new Vector3(0, 0, 0);
    float radiusOfSphereCast = 0.1f;
    float DIS_TO_TOES = 0.4f;
    TwoBoneIKConstraint ik;

    void Start()
    {
        ik = GetComponent<TwoBoneIKConstraint>();
    }

    // Update is called once per frame
    void Update()
    {
        FindGround();
    }

    void FindGround()
    {

        float RAY_DIST_ABOVE_FOOT = .5f;
        float RAY_LENGTH = 1f;

        Vector3 directionToToeRaycastOrigin = ik.data.hint.position - transform.position;
        directionToToeRaycastOrigin.y = 0;
        directionToToeRaycastOrigin.Normalize();

        Ray ray = new Ray(transform.position + Vector3.up * RAY_DIST_ABOVE_FOOT, Vector3.down);

        bool heelHit = Physics.SphereCast(ray, radiusOfSphereCast, out RaycastHit hitHeel, RAY_LENGTH);
        Debug.DrawRay(ray.origin, ray.direction * RAY_LENGTH);

        ray.origin += directionToToeRaycastOrigin * DIS_TO_TOES;

        bool toeHit = Physics.SphereCast(ray, radiusOfSphereCast, out RaycastHit hitToe, RAY_LENGTH);
        Debug.DrawRay(ray.origin, ray.direction * RAY_LENGTH);

        if (heelHit && toeHit)
        {
            // Foot forward vector
            Vector3 vectorToToe = hitToe.point - hitHeel.point;

            // move foot to ground
            Vector3 groundFootPosition = transform.position;
            groundFootPosition.y = hitHeel.point.y;
            transform.position = groundFootPosition + raycastOffset;
            
            // rotate foot so toes point at hitToe and ankle up is hitHeel's normal
            /*transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(vectorToToe, hitHeel.normal),
                90 * Time.deltaTime);*/
        } else
        {
            // ray doesn't hit
            if (ground != null)
            {
                transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z) + raycastOffset;
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}
