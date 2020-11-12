//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Raphael Lopes Baldi & Aquiris Game Experience LTDA.
//
//=============================================================================

using UnityEngine;

public struct MeshInstance
{
    public Mesh mesh;
    public int subMeshIndex;
    public Matrix4x4 transform;
}

public static class MeshCombineUtility
{
    public static Mesh Combine(MeshInstance[] combines, bool generateStrips)
    {
        int vertexCount = 0;
        int triangleCount = 0;
        int stripCount = 0;

        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                continue;

            vertexCount += combines[i].mesh.vertexCount;

            if (!generateStrips)
                continue;

            // Suboptimal for performance
            int curStripCount = combines[i].mesh.GetTriangles(combines[i].subMeshIndex).Length;
            if (curStripCount != 0)
            {
                if (stripCount != 0)
                {
                    if ((stripCount & 1) == 1)
                        stripCount += 3;
                    else
                        stripCount += 2;
                }
                stripCount += curStripCount;
            }
            else
            {
                generateStrips = false;
            }
        }

        // Precomputed how many triangles we need instead
        if (!generateStrips)
        {
            for (int i = 0; i < combines.Length; i++)
            {
                if (combines[i].mesh)
                {
                    triangleCount += combines[i].mesh.GetTriangles(combines[i].subMeshIndex).Length;
                }
            }
        }

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Vector4[] tangents = new Vector4[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        Vector2[] uv1 = new Vector2[vertexCount];
        Vector2[] uv2 = new Vector2[vertexCount];
        Color[] colors = new Color[vertexCount];

        int[] triangles = new int[triangleCount];
        int[] strip = new int[stripCount];

        int offset = 0;

        for (int i = 0; i < combines.Length; i++)
        {
            if (combines[i].mesh)
                Copy(combines[i].mesh.vertexCount, combines[i].mesh.vertices, vertices, ref offset, combines[i].transform);
        }

        offset = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                continue;

            Matrix4x4 invTranspose = combines[i].transform;
            invTranspose = invTranspose.inverse.transpose;
            CopyNormal(combines[i].mesh.vertexCount, combines[i].mesh.normals, normals, ref offset, invTranspose);
        }

        offset = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                continue;

            Matrix4x4 invTranspose = combines[i].transform;
            invTranspose = invTranspose.inverse.transpose;
            CopyTangents(combines[i].mesh.vertexCount, combines[i].mesh.tangents, tangents, ref offset, invTranspose);

        }

        offset = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            if (combines[i].mesh)
                Copy(combines[i].mesh.vertexCount, combines[i].mesh.uv, uv, ref offset);
        }

        offset = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            if (combines[i].mesh)
                Copy(combines[i].mesh.vertexCount, combines[i].mesh.uv2, uv1, ref offset);
        }

        float volume = 0.0f;
        for (int i = 0; i < combines.Length; i++)
        {
            if (combines[i].mesh)
                volume += combines[i].mesh.bounds.size.x * combines[i].mesh.bounds.size.y * combines[i].mesh.bounds.size.z;
        }

        offset = 0;
        float cVolume = 0.0f;
        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                continue;

            float inf = (combines[i].mesh.bounds.size.x * combines[i].mesh.bounds.size.y * combines[i].mesh.bounds.size.z) / volume;
            CopyUV2(combines[i].mesh.vertexCount, combines[i].mesh.uv2, uv2, ref offset, inf, cVolume);
            cVolume += inf;
        }

        offset = 0;
        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                CopyColors(combines[i].mesh.vertexCount, combines[i].mesh.colors, colors, ref offset);
        }

        int triangleOffset = 0;
        int stripOffset = 0;
        int vertexOffset = 0;

        for (int i = 0; i < combines.Length; i++)
        {
            if (!combines[i].mesh)
                continue;

            if (generateStrips)
            {
                int[] inputStrip = combines[i].mesh.GetTriangles(combines[i].subMeshIndex);
                if (stripOffset != 0)
                {
                    if ((stripOffset & 1) == 1)
                    {
                        strip[stripOffset + 0] = strip[stripOffset - 1];
                        strip[stripOffset + 1] = inputStrip[0] + vertexOffset;
                        strip[stripOffset + 2] = inputStrip[0] + vertexOffset;
                        stripOffset += 3;
                    }
                    else
                    {
                        strip[stripOffset + 0] = strip[stripOffset - 1];
                        strip[stripOffset + 1] = inputStrip[0] + vertexOffset;
                        stripOffset += 2;
                    }
                }

                for (int j = 0; j < inputStrip.Length; j++)
                {
                    strip[j + stripOffset] = inputStrip[j] + vertexOffset;
                }
                stripOffset += inputStrip.Length;
            }
            else
            {
                int[] inputTriangles = combines[i].mesh.GetTriangles(combines[i].subMeshIndex);
                for (int j = 0; j < inputTriangles.Length; j++)
                {
                    triangles[j + triangleOffset] = inputTriangles[j] + vertexOffset;
                }
                triangleOffset += inputTriangles.Length;
            }

            vertexOffset += combines[i].mesh.vertexCount;
        }

        Mesh mesh = new Mesh
        {
            name = "Combined Mesh",
            vertices = vertices,
            normals = normals,
            colors = colors,
            uv = uv,
            uv2 = uv1
        };

        mesh.uv2 = uv2;
        mesh.tangents = tangents;

        if (generateStrips && strip.Length % 3 == 0)
        {
            mesh.SetTriangles(strip, 0);
        }
        else
            mesh.triangles = triangles;

        return mesh;
    }

    private static void Copy(int vertexCount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = transform.MultiplyPoint(src[i]);
        offset += vertexCount;
    }

    private static void CopyNormal(int vertexCount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
        offset += vertexCount;
    }

    private static void Copy(int vertexCount, Vector2[] src, Vector2[] dst, ref int offset)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = src[i];
        offset += vertexCount;
    }

    private static void CopyUV2(int vertexCount, Vector2[] src, Vector2[] dst, ref int offset, float influence, float x)
    {
        for (int i = 0; i < src.Length; i++)
        {
            dst[i + offset] = src[i] * influence + new Vector2(x, 0);
        }

        offset += vertexCount;
    }

    private static void CopyColors(int vertexCount, Color[] src, Color[] dst, ref int offset)
    {
        for (int i = 0; i < src.Length; i++)
            dst[i + offset] = src[i];
        offset += vertexCount;
    }

    private static void CopyTangents(int vertexCount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
    {
        for (int i = 0; i < src.Length; i++)
        {
            Vector4 p4 = src[i];
            Vector3 p = new Vector3(p4.x, p4.y, p4.z);
            p = transform.MultiplyVector(p).normalized;
            dst[i + offset] = new Vector4(p.x, p.y, p.z, p4.w);
        }

        offset += vertexCount;
    }
}