using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Low-poly Icosphere mesh oluşturucu. 
/// LowPolyFoliage ve diğer scriptler tarafından köşeli küre mesh'i üretmek için kullanılır.
/// </summary>
public static class LowPolyMeshGenerator
{
    /// <summary>
    /// Belirtilen detay seviyesinde bir Icosphere mesh oluşturur.
    /// detaySeviyesi = 0 → 20 yüzey (ham ikosahedron)
    /// detaySeviyesi = 1 → 80 yüzey
    /// detaySeviyesi = 2 → 320 yüzey
    /// </summary>
    public static Mesh CreateIcosphere(int detaySeviyesi = 1)
    {
        // --- İkosahedron temel noktaları ---
        float t = (1f + Mathf.Sqrt(5f)) / 2f;

        List<Vector3> vertices = new List<Vector3>
        {
            Normalize(new Vector3(-1,  t,  0)),
            Normalize(new Vector3( 1,  t,  0)),
            Normalize(new Vector3(-1, -t,  0)),
            Normalize(new Vector3( 1, -t,  0)),
            Normalize(new Vector3( 0, -1,  t)),
            Normalize(new Vector3( 0,  1,  t)),
            Normalize(new Vector3( 0, -1, -t)),
            Normalize(new Vector3( 0,  1, -t)),
            Normalize(new Vector3( t,  0, -1)),
            Normalize(new Vector3( t,  0,  1)),
            Normalize(new Vector3(-t,  0, -1)),
            Normalize(new Vector3(-t,  0,  1))
        };

        List<int> indices = new List<int>
        {
            0,11, 5,  0, 5, 1,  0, 1, 7,  0, 7,10,  0,10,11,
            1, 5, 9,  5,11, 4, 11,10, 2, 10, 7, 6,  7, 1, 8,
            3, 9, 4,  3, 4, 2,  3, 2, 6,  3, 6, 8,  3, 8, 9,
            4, 9, 5,  2, 4,11,  6, 2,10,  8, 6, 7,  9, 8, 1
        };

        // --- Subdivide (alt bölme) ---
        for (int s = 0; s < detaySeviyesi; s++)
        {
            List<int> newIndices = new List<int>();
            Dictionary<long, int> midpointCache = new Dictionary<long, int>();

            for (int i = 0; i < indices.Count; i += 3)
            {
                int a = indices[i];
                int b = indices[i + 1];
                int c = indices[i + 2];

                int ab = GetMidpoint(a, b, vertices, midpointCache);
                int bc = GetMidpoint(b, c, vertices, midpointCache);
                int ca = GetMidpoint(c, a, vertices, midpointCache);

                newIndices.AddRange(new[] { a, ab, ca });
                newIndices.AddRange(new[] { b, bc, ab });
                newIndices.AddRange(new[] { c, ca, bc });
                newIndices.AddRange(new[] { ab, bc, ca });
            }
            indices = newIndices;
        }

        // --- Low-poly: Her üçgen için ayrı vertex (flat shading) ---
        Vector3[] flatVerts = new Vector3[indices.Count];
        int[] flatTris = new int[indices.Count];

        for (int i = 0; i < indices.Count; i++)
        {
            flatVerts[i] = vertices[indices[i]];
            flatTris[i] = i;
        }

        Mesh mesh = new Mesh();
        mesh.name = "LowPolyIcosphere";
        mesh.vertices = flatVerts;
        mesh.triangles = flatTris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

    private static Vector3 Normalize(Vector3 v)
    {
        return v.normalized;
    }

    private static int GetMidpoint(int a, int b, List<Vector3> verts, Dictionary<long, int> cache)
    {
        long key = a < b ? ((long)a << 32) | (uint)b : ((long)b << 32) | (uint)a;
        if (cache.TryGetValue(key, out int idx)) return idx;

        Vector3 mid = Normalize((verts[a] + verts[b]) * 0.5f);
        idx = verts.Count;
        verts.Add(mid);
        cache[key] = idx;
        return idx;
    }
}
