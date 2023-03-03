using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float speed = 10;
    float turn_speed = 100;
    float scan_distance = 0.75f;
    float hover_height = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1))
        {
            Debug.Log("Died");
            transform.position = new Vector3(0, hover_height, 0);
            transform.rotation = Quaternion.identity;
            return;
        }

        if (Input.GetAxis("Horizontal") < -0.5)
        {
            transform.Rotate(new Vector3(0, -turn_speed * Time.deltaTime, 0));
        }
        if (Input.GetAxis("Horizontal") > 0.5)
        {
            transform.Rotate(new Vector3(0, +turn_speed * Time.deltaTime, 0));
        }

        transform.position += speed * Time.deltaTime * transform.forward;

        var frontPosition = GetGroundPosition(transform.forward);
        var backPosition = GetGroundPosition(-transform.forward);
        var rightPosition = GetGroundPosition(transform.right);
        var leftPosition = GetGroundPosition(-transform.right);

        if (frontPosition != null && backPosition != null && rightPosition != null && leftPosition != null)
        {
            var mid = ((Vector3)frontPosition + (Vector3)backPosition + (Vector3)rightPosition + (Vector3)leftPosition) / 4f;
            var cross = Vector3.Cross((Vector3)(frontPosition - backPosition), (Vector3)(rightPosition - leftPosition)).normalized;
            //transform.position = mid + (cross * hover_height);
            var fwd = ((Vector3)frontPosition - (Vector3)backPosition);
            transform.rotation = Quaternion.LookRotation(fwd, cross);
        }

 
        

        
    }

    private Vector3? GetGroundPosition(Vector3 dir)
    {
        var ray = new Ray(transform.position + (dir * scan_distance), -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, scan_distance))
        {
            return hit.point;
        }
        return null;
    }
}
