using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Overlays;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;

    float speed = 10;
    float ground_turn_speed = 90;
    float air_turn_speed = 30;
    float fall_turn_speed = 60;
    float pullup_turn_speed = 180;
    float down_scan_distance = 1.5f;
    float forward_scan_distance = 0.5f;
    float air_ray_distance = 1000f;
    float height_offset = 1f;

    float pullup_turn_radius => speed / (pullup_turn_speed * Mathf.Deg2Rad);


    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }

    void Update()
    {
        if (IsDead())
        {
            Kill();
            return;
        }

        MoveForward();

        var centrePosition = GetGroundTarget(Vector3.zero);

        if (centrePosition != null)
        {
            GroundMovement((Vector3)centrePosition);
        }
        else
        {
            AirMovement();
        }
    }

    private bool IsDead()
    {
        return Physics.Raycast(transform.position + (transform.up * height_offset), transform.forward, forward_scan_distance);
    }

    private void Kill()
    {
        Debug.Log("Died");
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
    }

    private void MoveForward()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void GroundMovement(Vector3 center)
    {
        transform.position = center;
        transform.Rotate(new Vector3(0, ground_turn_speed * Time.deltaTime * GetTurnInputMultiplier(), 0));

        var front = GetGroundTarget(transform.forward);
        var back = GetGroundTarget(-transform.forward);
        var right = GetGroundTarget(transform.right);
        var left = GetGroundTarget(-transform.right);

        var forwardVector = transform.forward;
        if (front != null && back != null)
        {
            forwardVector = (Vector3)front - (Vector3)back;
        }
        else if (front == null && back != null)
        {
            forwardVector = center - (Vector3)back;
        }
        else if (front != null && back == null)
        {
            forwardVector = (Vector3)front - center;
        }

        var rightVector = transform.right;
        if (left != null && right != null)
        {
            rightVector = (Vector3)right - (Vector3)left;
        }
        else if (left != null && right == null)
        {
            rightVector = center - (Vector3)left;
        }
        else if (left == null && right != null)
        {
            rightVector = (Vector3)right - center;
        }

        var cross = Vector3.Cross(forwardVector, rightVector).normalized;
        var targetRotation = Quaternion.LookRotation(forwardVector, cross);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pullup_turn_speed * Time.deltaTime);
    }

    private void AirMovement()
    {
        transform.Rotate(new Vector3(0, air_turn_speed * Time.deltaTime * GetTurnInputMultiplier(), 0));

        if (!TryLand())
        {
            var gravityDirection = Quaternion.Euler(89, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, gravityDirection, fall_turn_speed * Time.deltaTime);
        }
    }

    private Vector3? GetGroundTarget(Vector3 dir)
    {
        var ray = new Ray(transform.position + (dir * down_scan_distance) + (transform.up * height_offset), -transform.up);
        return Physics.Raycast(ray, out RaycastHit hit, down_scan_distance) ? hit.point : null;
    }

    private RaycastHit? GetAirTarget(Quaternion offset)
    {
        var ray = new Ray(transform.position, offset * transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, air_ray_distance) ? hit : null;
    }

    private bool TryLand()
    {
        var forward = GetAirTarget(Quaternion.identity);
        var up = GetAirTarget(Quaternion.AngleAxis(1f, transform.right));
        var left = GetAirTarget(Quaternion.AngleAxis(1f, transform.up));
        var right = GetAirTarget(Quaternion.AngleAxis(-1f, transform.up));

        if (forward == null || up == null || left == null || right == null) return false;

        var groundVector = forward.Value.point - up.Value.point;
        var angleOfApproach = Vector3.Angle(transform.forward, groundVector);
        var pullupDistance = pullup_turn_radius * Mathf.Sin(angleOfApproach * Mathf.Deg2Rad);

        if (forward.Value.distance > pullupDistance) return false;

        var planeUp = Vector3.Cross(groundVector, (left.Value.point - right.Value.point));
        var targetRotation = Quaternion.LookRotation(groundVector, planeUp);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pullup_turn_speed * Time.deltaTime);

        return true;
    }

    private int GetTurnInputMultiplier()
    {
        if (Input.GetAxis("Horizontal") < -0.5 || Input.GetKey(KeyCode.LeftArrow))
        {
            return -1;
        }
        if (Input.GetAxis("Horizontal") > 0.5 || Input.GetKey(KeyCode.RightArrow))
        {
            return 1;
        }
        return 0;
    }
}
