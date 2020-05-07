using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFeetWalking : MonoBehaviour
{
    public Transform StickyTarget;
    float MoveDistanceThreshold = 1.5f;

    public StickyFeetWalking FootToWatch;

    [Header("Foot Animation")]
    public AnimationCurve footLateralEase; // how far it goes from side to side
    public AnimationCurve footLateralRaise; // how high it is
    public float timeToMoveFoot = 0.25f;

    float footMoveTimer = 0;

    Vector3 PlantedStart;
    Vector3 PlantedEnd;

    void Update()
    {
        CheckIfFootCanMove();
        AnimateFoot();
    }
    void AnimateFoot()
    {
        if (!IsAnimating())
        {
            transform.position = PlantedEnd;
            return;
        }

        footMoveTimer += Time.deltaTime;

        float p = footMoveTimer / timeToMoveFoot;
        p = Mathf.Clamp(p, 0, 1);
        Vector3 pos = Vector3.Lerp(PlantedStart, PlantedEnd, footLateralEase.Evaluate(p));
        pos.y += footLateralRaise.Evaluate(p);

        transform.position = pos;
    }
    void CheckIfFootCanMove()
    {
        if (IsAnimating()) return;

        if (FootToWatch.IsAnimating()) return;

        float d2 = (transform.position - StickyTarget.position).sqrMagnitude;
        if (d2 > MoveDistanceThreshold * MoveDistanceThreshold)
        {
            DoRayCast();
        }
    }
    void DoRayCast()
    {
        Ray ray = new Ray(StickyTarget.position + Vector3.up * 2.5f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            footMoveTimer = 0;
            PlantedStart = transform.position;
            PlantedEnd = hit.point + Vector3.up * 1.75f;
        }
    }
    bool IsAnimating()
    {
        return (footMoveTimer < timeToMoveFoot);
    }
}
