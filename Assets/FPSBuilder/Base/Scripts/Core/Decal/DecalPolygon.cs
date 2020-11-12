//=========== Copyright (c) GameBuilders, All rights reserved. ================
//
// Raphael Lopes Baldi & Aquiris Game Experience LTDA.
//
//=============================================================================

using UnityEngine;

public sealed class DecalPolygon
{
    public int vertexCount;
    public Vector3[] normal;
    public Vector3[] vertex;
    public Vector4[] tangent;

    public DecalPolygon()
    {
        vertexCount = 0;
        vertex = new Vector3[9];
        normal = new Vector3[9];
        tangent = new Vector4[9];
    }

    public static DecalPolygon ClipPolygonAgainstPlane(DecalPolygon polygon, Vector4 plane)
    {
        bool[] neg = new bool[10];
        int negCount = 0;

        Vector3 n = new Vector3(plane.x, plane.y, plane.z);

        for (int i = 0; i < polygon.vertexCount; i++)
        {
            neg[i] = (Vector3.Dot(polygon.vertex[i], n) + plane.w) < 0.0f;
            if (neg[i])
                negCount++;
        }

        if (negCount == polygon.vertexCount)
            return null;

        if (negCount == 0)
            return polygon;

        DecalPolygon tempPolygon = new DecalPolygon
        {
            vertexCount = 0
        };

        for (int i = 0; i < polygon.vertexCount; i++)
        {
            int b = (i == 0) ? polygon.vertexCount - 1 : i - 1;

            Vector3 v1;
            Vector3 v2;
            Vector3 dir;
            float t;

            if (neg[i])
            {
                if (neg[b])
                    continue;

                v1 = polygon.vertex[i];
                v2 = polygon.vertex[b];
                dir = (v2 - v1).normalized;

                t = -(Vector3.Dot(n, v1) + plane.w) / Vector3.Dot(n, dir);

                tempPolygon.tangent[tempPolygon.vertexCount] = polygon.tangent[i] + ((polygon.tangent[b] - polygon.tangent[i]).normalized * t);
                tempPolygon.vertex[tempPolygon.vertexCount] = v1 + ((v2 - v1).normalized * t);
                tempPolygon.normal[tempPolygon.vertexCount] = polygon.normal[i] + ((polygon.normal[b] - polygon.normal[i]).normalized * t);

                tempPolygon.vertexCount++;
            }
            else
            {
                if (neg[b])
                {
                    v1 = polygon.vertex[b];
                    v2 = polygon.vertex[i];
                    dir = (v2 - v1).normalized;

                    t = -(Vector3.Dot(n, v1) + plane.w) / Vector3.Dot(n, dir);

                    tempPolygon.tangent[tempPolygon.vertexCount] = polygon.tangent[b] + ((polygon.tangent[i] - polygon.tangent[b]).normalized * t);
                    tempPolygon.vertex[tempPolygon.vertexCount] = v1 + ((v2 - v1).normalized * t);
                    tempPolygon.normal[tempPolygon.vertexCount] = polygon.normal[b] + ((polygon.normal[i] - polygon.normal[b]).normalized * t);

                    tempPolygon.vertexCount++;
                }

                tempPolygon.tangent[tempPolygon.vertexCount] = polygon.tangent[i];
                tempPolygon.vertex[tempPolygon.vertexCount] = polygon.vertex[i];
                tempPolygon.normal[tempPolygon.vertexCount] = polygon.normal[i];

                tempPolygon.vertexCount++;
            }
        }

        return tempPolygon;
    }
}