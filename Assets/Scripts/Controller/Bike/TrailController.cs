using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrailController
{
    private readonly List<Vector3> vertices = new List<Vector3>();
    private readonly List<Vector3> pastPoints = new List<Vector3>();
    private readonly List<int> triangles = new List<int>();
    private readonly MeshFilter meshFilter;
    private readonly MeshRenderer meshRenderer;
    private readonly MeshCollider meshCollider;
    private readonly Transform transform;
    private int n = 0;

    private const float TRAIL_HEIGHT = 0.8f;
    private const float TRAIL_WIDTH = 0.2f;
    private const float HALF_WIDTH = TRAIL_WIDTH / 2f;
    private const float BACK_OFFSET = 0.25f;
    private const float MIN_POLY_DIST = 1f;


    public TrailController(Player player, Transform transform, Material material)
    {
        this.transform = transform;
        var trailGo = new GameObject("Trail");
        var trailMat = new Material(material);
        var c = player.Color;
        c.a = 0.5f;
        trailMat.color = c;

        meshFilter = trailGo.AddComponent<MeshFilter>();
        meshRenderer = trailGo.AddComponent<MeshRenderer>();
        meshRenderer.material = trailMat;
        meshCollider = trailGo.AddComponent<MeshCollider>();
        meshCollider.cookingOptions = MeshColliderCookingOptions.None;
        trailGo.layer = 3;

        AddTrail();
    }

    public void ClearTrail()
    {
        vertices.Clear();
        triangles.Clear();
        n = 0;
        meshFilter.mesh.Clear();
        meshCollider.sharedMesh?.Clear();

        AddTrail();
    }

    public void AddTrail()
    {
        var x = HALF_WIDTH * transform.right;
        var y = TRAIL_HEIGHT * transform.up;
        var p = transform.position - BACK_OFFSET * transform.forward;
        var removedPoints = false;

        if (n >= 8)
        {
            var m = pastPoints.Count;

            if (Vector3.Distance(p, pastPoints[m - 2]) < MIN_POLY_DIST)
            {
                vertices.RemoveRange(n - 4, 4);
                pastPoints.RemoveAt(m - 1);
                removedPoints = true;
            }
        }

        vertices.Add(p + y - x); // top left
        vertices.Add(p + y + x); // top right
        vertices.Add(p - x); // bottom left
        vertices.Add(p + x); // bottom right
        pastPoints.Add(p);
        n = vertices.Count;

        if (n >= 8 && !removedPoints)
        {
            // top 1
            triangles.Add(n - 4);
            triangles.Add(n - 7);
            triangles.Add(n - 8);
            // top 2
            triangles.Add(n - 4);
            triangles.Add(n - 3);
            triangles.Add(n - 7);
            // left 1
            triangles.Add(n - 2);
            triangles.Add(n - 4);
            triangles.Add(n - 8);
            // left 2
            triangles.Add(n - 6);
            triangles.Add(n - 2);
            triangles.Add(n - 8);
            // right 1
            triangles.Add(n - 1);
            triangles.Add(n - 7);
            triangles.Add(n - 3);
            // right 2
            triangles.Add(n - 1);
            triangles.Add(n - 5);
            triangles.Add(n - 7);

            meshFilter.mesh.vertices = vertices.ToArray();
            meshFilter.mesh.triangles = triangles.ToArray();
            meshCollider.sharedMesh = meshFilter.mesh;
        }   
    }
}
