using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    float speed = 10;
    float ground_turn_speed = 90;
    float air_turn_speed = 30;
    float pullup_turn_speed = 180;
    float scan_distance = 0.75f;
    Vector3 scan_offset = new Vector3(0, 0.5f, 0);

    float pullup_turn_radius => speed / (pullup_turn_speed * Mathf.Deg2Rad);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position + scan_offset, transform.forward, scan_distance))
        {
            Debug.Log("Died");
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.identity;
            return;
        }

        transform.position += speed * Time.deltaTime * transform.forward;

        var frontPosition = GetGroundPosition(transform.forward * 0.1f);
        var backPosition = GetGroundPosition(-transform.forward * 0.1f);
        var rightPosition = GetGroundPosition(transform.right * 0.1f);
        var leftPosition = GetGroundPosition(-transform.right * 0.1f);

        bool isAirbourne;

        // on the ground
        if (frontPosition != null && backPosition != null && rightPosition != null && leftPosition != null)
        {

            isAirbourne = false;
            var cross = Vector3.Cross((Vector3)(frontPosition - backPosition), (Vector3)(rightPosition - leftPosition)).normalized;
            var fwd = ((Vector3)frontPosition - (Vector3)backPosition);
            transform.rotation = Quaternion.LookRotation(fwd, cross);
        }
        // airbourne
        else
        {
            isAirbourne = true;

            var gravityDirection = Quaternion.Euler(89, transform.rotation.eulerAngles.y, 0);
            

            var airForwardRay = new Ray(transform.position, transform.forward);
            var airUpRay = new Ray(transform.position, Quaternion.AngleAxis(1f, transform.right) * transform.forward);
            var airLeftRay = new Ray(transform.position, Quaternion.AngleAxis(1f, transform.up) * transform.forward);
            var airRightRay = new Ray(transform.position, Quaternion.AngleAxis(-1f, transform.up) * transform.forward);

            var isAirForward = Physics.Raycast(airForwardRay, out RaycastHit airForwardHit, 1000f);
            var isAirUp = Physics.Raycast(airUpRay, out RaycastHit airUpHit, 1000f);
            var isAirLeft = Physics.Raycast(airLeftRay, out RaycastHit airLeftHit, 1000f);
            var isAirRight = Physics.Raycast(airRightRay, out RaycastHit airRightHit, 1000f);

            if (isAirUp && isAirForward && isAirLeft && isAirRight)
            {
                var groundVector = (airForwardHit.point - airUpHit.point);
                var angleOfApproach = Vector3.Angle(transform.forward, groundVector);
                var pullupDistance = pullup_turn_radius * Mathf.Sin(angleOfApproach * Mathf.Deg2Rad);

                if (airForwardHit.distance < pullupDistance)
                {                  
                    var planeUp = Vector3.Cross(groundVector, (airLeftHit.point - airRightHit.point));
                    var targetRotation = Quaternion.LookRotation(groundVector, planeUp);
                    //transform.position = airForwardHit.point;
                    //transform.rotation = targetRotation;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pullup_turn_speed * Time.deltaTime);

                } else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, gravityDirection, air_turn_speed * Time.deltaTime);
                }
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, gravityDirection, air_turn_speed * Time.deltaTime);
            }

            
        }

        var turn_speed = isAirbourne ? air_turn_speed : ground_turn_speed;

        if (Input.GetAxis("Horizontal") < -0.5 || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, -turn_speed * Time.deltaTime, 0));
        }
        if (Input.GetAxis("Horizontal") > 0.5 || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, +turn_speed * Time.deltaTime, 0));
        }


    }

    private Vector3? GetGroundPosition(Vector3 dir)
    {
        var ray = new Ray(transform.position + (dir * scan_distance) + scan_offset, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, scan_distance))
        {
            return hit.point;
        }
        return null;
    }
}
