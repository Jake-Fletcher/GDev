//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Raphael Lopes Baldi & Aquiris Game Experience LTDA.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

public struct DecalPresets
{
    public float maxAngle;
    public float pushDistance;
    public Material material;
}

public class Decal : MonoBehaviour
{
    private float angleCosine;

    private Vector4 bottomPlane;
    private Vector4 topPlane;
    private Vector4 leftPlane;
    private Vector4 rightPlane;
    private Vector4 frontPlane;
    private Vector4 backPlane;

    private Vector3 decalNormal;
    private Vector3 decalCenter;
    private Vector3 decalTangent;
    private Vector3 decalBinormal;
    private Vector3 decalSize;

    private List<DecalPolygon> startPolygons;
    private List<DecalPolygon> clippedPolygons;

    private List<MeshInstance> m_InstancesList;

    public void Calculate(DecalPresets presets, GameObject affectedObject)
    {
        if (!affectedObject)
        {
            Debug.LogWarning("No object will be affected. Decal will not be calculated.");
            return;
        }

        angleCosine = Mathf.Cos(presets.maxAngle * Mathf.Deg2Rad);

        m_InstancesList = new List<MeshInstance>();
        CalculateObjectDecal(affectedObject);

        if (m_InstancesList.Count > 0)
        {
            MeshInstance[] instances = new MeshInstance[m_InstancesList.Count];
            for (int i = 0; i < instances.Length; i++)
            {
                instances[i] = m_InstancesList[i];
            }

            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (!meshRenderer)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.material = presets.material;

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

            if (!meshFilter)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            else
            {
                DestroyImmediate(meshFilter.sharedMesh);
            }

            Mesh finalMesh = MeshCombineUtility.Combine(instances, true);

            if (presets.pushDistance > 0.0f)
            {
                List<List<int>> relations = new List<List<int>>();
                Vector3[] vert = finalMesh.vertices;
                Vector3[] normals = finalMesh.normals;

                bool[] usedIndex = new bool[vert.Length];
                for (int i = 0; i < usedIndex.Length; i++)
                {
                    usedIndex[i] = false;
                }

                for (int i = 0; i < vert.Length; i++)
                {
                    if (usedIndex[i])
                        continue;

                    List<int> c = new List<int>
                    {
                        i
                    };

                    usedIndex[i] = true;

                    for (int j = i + 1; j < vert.Length; j++)
                    {
                        if (usedIndex[j])
                            continue;

                        if (Vector3.Distance(vert[i], vert[j]) > 0.001f)
                            continue;

                        c.Add(j);

                        usedIndex[j] = true;
                    }

                    relations.Add(c);
                }

                for (int i = 0, k = relations.Count; i < k; i++)
                {
                    Vector3 nNormal = Vector3.zero;
                    foreach (int j in relations[i])
                    {
                        nNormal += normals[j];
                    }

                    nNormal = (nNormal / relations[i].Count).normalized;

                    foreach (int j in relations[i])
                    {
                        vert[j] += nNormal * (presets.pushDistance);
                    }
                }

                finalMesh.vertices = vert;
            }

            finalMesh.name = "DecalMesh";
            meshFilter.mesh = finalMesh;

            for (int i = 0; i < m_InstancesList.Count; i++)
            {
                DestroyImmediate(m_InstancesList[i].mesh);
            }
        }

        m_InstancesList.Clear();
        m_InstancesList = null;
    }

    private void CalculateObjectDecal(GameObject obj)
    {
        MeshFilter mf = obj.GetComponent<MeshFilter>();

        if (!mf)
            return;

        Mesh mesh = mf.sharedMesh;

        if (!mesh || mesh.name.ToLower().Contains("combined") || mesh.tangents.Length == 0)
        {
            return;
        }

        decalNormal = obj.transform.InverseTransformDirection(transform.forward);
        decalCenter = obj.transform.InverseTransformPoint(transform.position);
        decalTangent = obj.transform.InverseTransformDirection(transform.right);
        decalBinormal = obj.transform.InverseTransformDirection(transform.up);

        Vector3 lossyScale = obj.transform.lossyScale;
        Vector3 scale = transform.lossyScale;
        decalSize = new Vector3(scale.x / lossyScale.x, scale.y / lossyScale.y, scale.z / lossyScale.z);

        bottomPlane = new Vector4(-decalBinormal.x, -decalBinormal.y, -decalBinormal.z, (decalSize.y * 0.5f) + Vector3.Dot(decalCenter, decalBinormal));
        topPlane = new Vector4(decalBinormal.x, decalBinormal.y, decalBinormal.z, (decalSize.y * 0.5f) - Vector3.Dot(decalCenter, decalBinormal));
        rightPlane = new Vector4(-decalTangent.x, -decalTangent.y, -decalTangent.z, (decalSize.x * 0.5f) + Vector3.Dot(decalCenter, decalTangent));
        leftPlane = new Vector4(decalTangent.x, decalTangent.y, decalTangent.z, (decalSize.x * 0.5f) - Vector3.Dot(decalCenter, decalTangent));
        frontPlane = new Vector4(decalNormal.x, decalNormal.y, decalNormal.z, (decalSize.z * 0.5f) - Vector3.Dot(decalCenter, decalNormal));
        backPlane = new Vector4(-decalNormal.x, -decalNormal.y, -decalNormal.z, (decalSize.z * 0.5f) + Vector3.Dot(decalCenter, decalNormal));

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector4[] tangents = mesh.tangents;

        startPolygons = new List<DecalPolygon>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            int i1 = triangles[i];
            int i2 = triangles[i + 1];
            int i3 = triangles[i + 2];

            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];
            Vector3 v3 = vertices[i3];

            Vector3 n1 = normals[i1];

            float dot = Vector3.Dot(n1, -decalNormal);

            if (dot <= angleCosine)
                continue;

            Vector3 t1 = tangents[i1];
            Vector3 t2 = tangents[i2];
            Vector3 t3 = tangents[i3];

            DecalPolygon decalPolygon = new DecalPolygon
            {
                vertexCount = 3,
                vertex = {[0] = v1, [1] = v2, [2] = v3},
                normal = {[0] = n1, [1] = n1, [2] = n1},
                tangent = {[0] = t1, [1] = t2, [2] = t3}
            };

            startPolygons.Add(decalPolygon);
        }

        Mesh aux = CreateMesh(ClipMesh());

        if (aux)
        {
            MeshInstance instance = new MeshInstance
            {
                mesh = aux,
                subMeshIndex = 0,
                transform = transform.worldToLocalMatrix * obj.transform.localToWorldMatrix
            };
            m_InstancesList.Add(instance);
        }

        startPolygons.Clear();
        startPolygons = null;
        clippedPolygons.Clear();
        clippedPolygons = null;
    }

    private Mesh CreateMesh(int totalVertices)
    {
        if (clippedPolygons == null)
            return null;

        if (clippedPolygons.Count <= 0)
            return null;

        if (totalVertices < 3)
            return null;

        int[] newTris = new int[(totalVertices - 2) * 3];

        Vector3[] newVertices = new Vector3[totalVertices];
        Vector3[] newNormals = new Vector3[totalVertices];
        Vector2[] newUv = new Vector2[totalVertices];
        Vector4[] newTangents = new Vector4[totalVertices];

        int count = 0;
        int trisCount = 0;
        int oCount = 0;

        float one_over_w = 1.0f / decalSize.x;
        float one_over_h = 1.0f / decalSize.y;

        foreach (DecalPolygon p in clippedPolygons)
        {
            for (int i = 0; i < p.vertexCount; i++)
            {
                newVertices[count] = p.vertex[i];
                newNormals[count] = p.normal[i];

                newTangents[count] = p.tangent[i];

                if (i < p.vertexCount - 2)
                {
                    newTris[trisCount] = oCount;
                    newTris[trisCount + 1] = count + 1;
                    newTris[trisCount + 2] = count + 2;
                    trisCount += 3;
                }

                count++;
            }
            oCount = count;
        }

        for (int i = 0; i < newVertices.Length; i++)
        {
            Vector3 dir = newVertices[i] - decalCenter;

            float tempU = (Vector3.Dot(dir, decalTangent) * one_over_w);
            float tempV = (Vector3.Dot(dir, decalBinormal) * one_over_h);

            float u = tempU - 0.5f;
            float v = tempV + 0.5f;

            newUv[i] = new Vector2(u, v);
        }

        Mesh mesh = new Mesh
        {
            vertices = newVertices,
            normals = newNormals,
            triangles = newTris,
            uv = newUv,
            uv2 = newUv
        };
        mesh.uv2 = newUv;

        mesh.tangents = newTangents;

        return mesh;
    }

    private int ClipMesh()
    {
        int totalVertices = 0;

        if (clippedPolygons == null)
            clippedPolygons = new List<DecalPolygon>();
        else
            clippedPolygons.Clear();

        for (int i = 0, c = startPolygons.Count; i < c; i++)
        {
            DecalPolygon face = startPolygons[i];

            DecalPolygon tempFace = DecalPolygon.ClipPolygonAgainstPlane(face, frontPlane);
            if (tempFace == null)
                continue;

            tempFace = DecalPolygon.ClipPolygonAgainstPlane(tempFace, backPlane);
            if (tempFace == null)
                continue;

            tempFace = DecalPolygon.ClipPolygonAgainstPlane(tempFace, rightPlane);
            if (tempFace == null)
                continue;

            tempFace = DecalPolygon.ClipPolygonAgainstPlane(tempFace, leftPlane);
            if (tempFace == null)
                continue;

            tempFace = DecalPolygon.ClipPolygonAgainstPlane(tempFace, bottomPlane);
            if (tempFace == null)
                continue;

            tempFace = DecalPolygon.ClipPolygonAgainstPlane(tempFace, topPlane);
            if (tempFace == null)
                continue;

            totalVertices += tempFace.vertexCount;
            clippedPolygons.Add(tempFace);
        }
        return totalVertices;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}