using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeMover : MonoBehaviour
{
    private float unboosted_speed = 10;
    private float boosted_speed = 17;
    private float ground_turn_speed = 150;
    private float air_turn_speed = 30;
    private float fall_turn_speed = 60;
    private float pullup_turn_speed = 180;
    private float down_scan_distance = 1.5f;
    private float forward_scan_distance = 0.5f;
    private float air_ray_distance = 1000f;
    private float ground_height_offset = 1f;
    private float trail_height_offset = 0.5f;

    private LayerMask groundMask;
    private LayerMask trailMask;
    private IInputController input;

    public void Init(IInputController input)
    {
        this.input = input;
        groundMask = LayerMask.GetMask("Default");
        trailMask = LayerMask.GetMask("Trail");
    }

    private float GetPullUpTurnRadius() => unboosted_speed / (pullup_turn_speed * Mathf.Deg2Rad);

    public void UpdatePosition()
    {
        var speed = input.IsButton1Held() ? boosted_speed : unboosted_speed;
        MoveForward(speed);

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

    public bool IsColliding()
    {
        // ground in front
        if (Physics.Raycast(transform.position + (transform.up * ground_height_offset), transform.forward, forward_scan_distance, groundMask))
            return true;

        // trail in front
        if (Physics.Raycast(transform.position + (transform.up * trail_height_offset), transform.forward, forward_scan_distance, trailMask))
            return true;

        // trail below
        if (Physics.Raycast(transform.position + (transform.up * trail_height_offset), -transform.up, 1f, trailMask))
            return true;

        return false;
    }

    private void MoveForward(float moveSpeed)
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
    }

    private void GroundMovement(Vector3 center)
    {
        transform.position = center;
        transform.Rotate(new Vector3(0, ground_turn_speed * Time.deltaTime * input.GetXAxis(), 0));

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
        transform.Rotate(new Vector3(0, air_turn_speed * Time.deltaTime * input.GetXAxis(), 0));

        if (!TryLand())
        {
            var gravityDirection = Quaternion.Euler(89, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, gravityDirection, fall_turn_speed * Time.deltaTime);
        }
    }

    private Vector3? GetGroundTarget(Vector3 dir)
    {
        var ray = new Ray(transform.position + (dir * down_scan_distance) + (transform.up * ground_height_offset), -transform.up);
        return Physics.Raycast(ray, out RaycastHit hit, down_scan_distance, groundMask) ? hit.point : null;
    }

    private RaycastHit? GetAirTarget(Quaternion offset)
    {
        var ray = new Ray(transform.position, offset * transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, air_ray_distance, groundMask) ? hit : null;
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
        var pullupDistance = GetPullUpTurnRadius() * Mathf.Sin(angleOfApproach * Mathf.Deg2Rad);

        if (forward.Value.distance > pullupDistance) return false;

        var planeUp = Vector3.Cross(groundVector, (left.Value.point - right.Value.point));
        var targetRotation = Quaternion.LookRotation(groundVector, planeUp);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, pullup_turn_speed * Time.deltaTime);

        return true;
    }
}
