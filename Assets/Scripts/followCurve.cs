using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCurve : MonoBehaviour
{

    public BezierCurve curve;
    [Range(0, 1)] public float percent = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (curve != null)
        {
            percent += .1f * Time.deltaTime;
            if (percent >= 1) percent = 1;
            transform.position = curve.FindPositionAt(percent);
        }
    }
}
