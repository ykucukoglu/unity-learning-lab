using UnityEngine;

/// <summary>
/// Bu script, standart bir Unity küresini otomatik olarak 
/// LowPolyMeshGenerator'dan gelen köşeli (Icosphere) mesh ile değiştirir.
/// </summary>
[ExecuteInEditMode] 
[RequireComponent(typeof(MeshFilter))]
public class LowPolyFoliage : MonoBehaviour
{
    [Range(0, 2)]
    public int detaySeviyesi = 1;

    void OnEnable()
    {
        BakeMesh();
    }

    [ContextMenu("Mesh'i Yenile")]
    public void BakeMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            mf.sharedMesh = LowPolyMeshGenerator.CreateIcosphere(detaySeviyesi);
        }
    }
}
