using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class GraphicExtension
{
     public static Vector4 ToVector4(this Vector3 _vector, float _fill = 0) => new Vector4(_vector.x, _vector.y, _vector.z, _fill);
    public static Mesh Copy(this Mesh _srcMesh)
    {
        Mesh copy = new Mesh();
        CopyMesh(_srcMesh, copy);
        return copy;
    }
    public static T[] DeepCopy<T>(this T[] _srcArray)
    {
        T[] dstArray = new T[_srcArray.Length];
        for (int i = 0; i < _srcArray.Length; i++)
            dstArray[i] = _srcArray[i];
        return dstArray;
    }
    public static void CopyMesh(Mesh _src, Mesh _tar)
    {
        _tar.Clear();
        Vector3[] vertices = _src.vertices;
        _tar.vertices = vertices;
        _tar.normals = _src.normals;
        _tar.tangents = _src.tangents;
        _tar.name = _src.name;
        _tar.bounds = _src.bounds;
        _tar.bindposes = _src.bindposes;
        _tar.colors = _src.colors;
        _tar.boneWeights = _src.boneWeights;
        List<Vector4> uvs = new List<Vector4>();
        for (int i = 0; i < 8; i++)
        {
            _src.GetUVs(i, uvs);
            _tar.SetUVsResize(i, uvs);
        }

        _tar.subMeshCount = _src.subMeshCount;
        for (int i = 0; i < _src.subMeshCount; i++)
        {
            _tar.SetIndices(_src.GetIndices(i),MeshTopology.Triangles,i,false);
            _tar.SetSubMesh(i,_src.GetSubMesh(i),MeshUpdateFlags.DontRecalculateBounds);
        }

        _tar.ClearBlendShapes();
        _src.TraversalBlendShapes(vertices.Length, (name, index, frame, weight, deltaVertices, deltaNormals, deltaTangents) => _tar.AddBlendShapeFrame(name, weight, deltaVertices, deltaNormals, deltaTangents));
    }
    public static void SetUVsResize(this Mesh _tar, int _index, List<Vector4> uvs)
    {
        if (uvs.Count <= 0)
            return;
        bool third = false;
        bool fourth = false;
        for (int j = 0; j < uvs.Count; j++)
        {
            Vector4 check = uvs[j];
            third |= check.z != 0;
            fourth |= check.w != 0;
        }

        if (fourth)
            _tar.SetUVs(_index, uvs);
        else if (third)
            _tar.SetUVs(_index, uvs.Select(vec4 => new Vector3(vec4.x, vec4.y, vec4.z)).ToArray());
        else
            _tar.SetUVs(_index, uvs.Select(vec4 => new Vector2(vec4.x, vec4.y)).ToArray());
    }
    public static void TraversalBlendShapes(this Mesh _srcMesh, int _VertexCount, Action<string, int, int, float, Vector3[], Vector3[], Vector3[]> _OnEachFrame)
    {
        Vector3[] deltaVerticies = new Vector3[_VertexCount];
        Vector3[] deltaNormals = new Vector3[_VertexCount];
        Vector3[] deltaTangents = new Vector3[_VertexCount];
        for (int i = 0; i < _srcMesh.blendShapeCount; i++)
        {
            int frameCount = _srcMesh.GetBlendShapeFrameCount(i);
            string name = _srcMesh.GetBlendShapeName(i);
            for (int j = 0; j < frameCount; j++)
            {
                float weight = _srcMesh.GetBlendShapeFrameWeight(i, j);
                _srcMesh.GetBlendShapeFrameVertices(i, j, deltaVerticies, deltaNormals, deltaTangents);
                _OnEachFrame(name, i, j, weight, deltaVerticies, deltaNormals, deltaTangents);
            }
        }
    }
}
public interface ITriangle<T> where T : struct
{
    T this[int _index] { get; }
    T V0 { get;}
    T V1 { get; }
    T V2 { get; }
}
public struct Triangle<T>: ITriangle<T>,IEquatable<Triangle<T>>,IIterate<T> where T : struct
{
    public T v0;
    public T v1;
    public T v2;
    public int Length => 3;
    public T this[int _index]
    {
        get
        {
            switch (_index)
            {
                default: Debug.LogError("Invalid Index:" + _index); return v0;
                case 0: return v0;
                case 1: return v1;
                case 2: return v2;
            }
        }
    }
        
    public Triangle(T _v0, T _v1, T _v2)
    {
        v0 = _v0;
        v1 = _v1;
        v2 = _v2;
    }
    public Triangle((T v0,T v1,T v2) _tuple) : this(_tuple.v0,_tuple.v1,_tuple.v2)
    {
    }

    public T V0 => v0;
    public T V1 => v1;
    public T V2 => v2;
    #region Implements
    public bool Equals(Triangle<T> other)
    {
        return v0.Equals(other.v0) && v1.Equals(other.v1) && v2.Equals(other.v2);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = v0.GetHashCode();
            hashCode = (hashCode * 397) ^ v1.GetHashCode();
            hashCode = (hashCode * 397) ^ v2.GetHashCode();
            return hashCode;
        }
    }

    public bool Equals(Triangle<T> x, Triangle<T> y)
    {
        return x.v0.Equals(y.v0) && x.v1.Equals(y.v1) && x.v2.Equals(y.v2);
    }


    public int GetHashCode(Triangle<T> obj)
    {
        unchecked
        {
            int hashCode = obj.v0.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.v1.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.v2.GetHashCode();
            return hashCode;
        }
    }
    #endregion
}