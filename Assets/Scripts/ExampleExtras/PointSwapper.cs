using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example physics system interaction script: swapping points
/// </summary>
public class PointSwapper : MonoBehaviour
{
    public Transform tracked = null;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo))
            {
                if (tracked == null) tracked = hitInfo.transform;
                else
                {
                    var t = hitInfo.transform.position;
                    hitInfo.transform.position = tracked.position;
                    tracked.position = t;
                    tracked = null;
                }
            }
        }
    }
}
