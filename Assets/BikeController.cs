using System;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] List<GameObject> partsToColour;
    [SerializeField] GameObject body;

    private float speed = 10;
    private float ground_turn_speed = 150;
    private float air_turn_speed = 30;
    private float fall_turn_speed = 60;
    private float pullup_turn_speed = 180;
    private float down_scan_distance = 1.5f;
    private float forward_scan_distance = 0.5f;
    private float air_ray_distance = 1000f;
    private float ground_height_offset = 1f;
    private float trail_height_offset = 0.5f;

    private Player player;
    private TrailBuilder trailBuilder;

    private bool isMoving = false;
    private Transform spawn;
    private LayerMask groundMask;
    private LayerMask trailMask;

    public event Action OnDeath;

    public void Init(Player player, Transform spawn)
    {
        this.player = player;
        this.spawn = spawn;

        var mat = new Material(partsToColour[0].GetComponent<MeshRenderer>().material);
        mat.color = player.Color;
        partsToColour.ForEach(p => p.GetComponent<MeshRenderer>().material = mat);
        trailBuilder = GetComponent<TrailBuilder>();
        trailBuilder.Init(player);
        groundMask = LayerMask.GetMask("Default");
        trailMask = LayerMask.GetMask("Trail");
    }

    public Transform GetCameraTransform() => cameraTransform;

    public void ResetBikeStartOfRound()
    {
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        SetVisible(true);
        trailBuilder.ClearTrail();
    }

    public void StartMovingBikeStartOfRound()
    {
        trailBuilder.SetEnabled(true);
        isMoving = true;
    }

    public void StartMovingBikeForPlayerSelect()
    {
        isMoving = true;
    }

    public void PauseBikeAtEndOfRound()
    {
        isMoving = false;
    }

    private float GetPullUpTurnRadius() => speed / (pullup_turn_speed * Mathf.Deg2Rad);

    void Update()
    {
        if (!isMoving)
            return;

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

        trailBuilder.AddTrail();
    }

    private bool IsDead()
    {
        if (Physics.Raycast(transform.position + (transform.up * ground_height_offset), transform.forward, forward_scan_distance, groundMask))
            return true;

        return Physics.Raycast(transform.position + (transform.up * trail_height_offset), transform.forward, forward_scan_distance, trailMask);
    }

    private void Kill()
    {
        isMoving = false;
        SetVisible(false);
        OnDeath?.Invoke();
    }

    private void MoveForward()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void GroundMovement(Vector3 center)
    {
        transform.position = center;
        transform.Rotate(new Vector3(0, ground_turn_speed * Time.deltaTime * player.Joystick.GetXAxis(), 0));

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
        transform.Rotate(new Vector3(0, air_turn_speed * Time.deltaTime * player.Joystick.GetXAxis(), 0));

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

    private void SetVisible(bool isVisible)
    {
        body.SetActive(isVisible);
    }
}
