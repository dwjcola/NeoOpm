using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SmoothNormalsTool : Editor
{
    [MenuItem("Assets/Generate SmoothNormals")]
    static void GenerateSmoothNormals()
    {
        Mesh[] meshs = Selection.GetFiltered<Mesh>(SelectionMode.Deep);
        if (meshs.Length <= 0)
        {
            return;
        }
        Mesh _src = meshs[0];
        GenerateSmoothNormals(_src);
    }
    public static void GenerateSmoothNormals(Mesh _srcMesh)
    {
        Vector3[] verticies = _srcMesh.vertices;
        var groups = verticies.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        Vector3[] normals =  RenegerateNormals(_srcMesh.triangles,verticies);
        Vector3[] smoothNormals = normals.DeepCopy();
        foreach (var group in groups)
        {
            if (group.Count() == 1)
                continue;
            Vector3 smoothNormal = Vector3.zero;
            foreach (var index in group)
                smoothNormal += normals[index.Value];
            smoothNormal = smoothNormal.normalized;
            foreach (var index in group)
                smoothNormals[index.Value] = smoothNormal;
        }
        Mesh target = _srcMesh.Copy();
        target.SetTangents(smoothNormals.ToList().Select(p => p.ToVector4()).ToArray());
        AssetDatabase.CreateAsset(target, "Assets/aa_smooth.asset");
    }
    static Vector3[] RenegerateNormals(int[] _indices, Vector3[] _verticies)
    {
        Vector3[] normals = new Vector3[_verticies.Length];
        GTrianglePolygon[] polygons = GetPolygons(_indices);
        foreach(var polygon in polygons)
        {
            GTriangle triangle = new GTriangle(polygon.GetVertices(_verticies));
            Vector3 normal = triangle.normal;
            foreach (var index in polygon)
                normals[index] += normal;
        }
        normals=normals.Select(normal => normal.normalized).ToArray();
        return normals;
    }
    public static GTrianglePolygon[] GetPolygons(int[] _indices)
    {
        GTrianglePolygon[] polygons = new GTrianglePolygon[_indices.Length / 3];
        for (int i = 0; i < polygons.Length; i++)
        {
            int startIndex = i * 3;
            int triangle0 = _indices[startIndex];
            int triangle1 = _indices[startIndex + 1];
            int triangle2 = _indices[startIndex + 2];
            polygons[i] = new GTrianglePolygon(triangle0, triangle1, triangle2);
        }
        return polygons;
    }
    
}
[Serializable]
public struct GTrianglePolygon:IEnumerable<int>,IIterate<int>
{
    public int index0;
    public int index1;
    public int index2;

    public GTrianglePolygon(int _index0, int _index1, int _index2)
    {
        index0 = _index0;
        index1 = _index1;
        index2 = _index2;
    }
    public (T v0, T v1, T v2) GetVertices<T>(IList<T> _vertices) => (_vertices[index0], _vertices[index1],_vertices[index2]);
    public (Y v0, Y v1, Y v2) GetVertices<T,Y>(IList<T> _vertices, Func<T, Y> _getVertex) => ( _getVertex( _vertices[index0]), _getVertex(_vertices[index1]),_getVertex( _vertices[index2]));
    public IEnumerable<T> GetEnumerator<T>(IList<T> _vertices)
    {
        yield return _vertices[index0];
        yield return _vertices[index1];
        yield return _vertices[index2];
    }
    public IEnumerator<int> GetEnumerator()
    {
        yield return index0;
        yield return index1;
        yield return index2;
    }

    IEnumerator IEnumerable.GetEnumerator()=>GetEnumerator();

    public int Length => 3;
    public int this[int _index]
    {
        get
        {
            switch (_index)
            {
                default: throw new Exception("Invalid Index:" + _index);
                case 0: return index0;
                case 1: return index1;
                case 2: return index2;
            }
        }
    }

}
public static class TSPoolCollection<T> where T:new()
    {
        private static readonly MethodInfo kClearMethod = typeof(T).GetMethod("Clear");
        private static Stack<T> m_PoolItems { get; set; } = new Stack<T>();
        public static T Spawn()
        {
            T collection=m_PoolItems.Count > 0?m_PoolItems.Pop():new T();
            kClearMethod.Invoke(collection,null);
            return collection;
        }
        public static void Recycle(T item)
        {
            m_PoolItems.Push(item);
        }
    }

    public static class TSPoolList<T>
    {
        public static List<T> Spawn() => TSPoolCollection<List<T>>.Spawn();
        public static void Spawn(out List<T> _list) =>_list=TSPoolCollection<List<T>>.Spawn();
        public static void Recycle(List<T> _list) => TSPoolCollection<List<T>>.Recycle(_list);
    }
  
[Serializable]
public struct GTriangle:ITriangle<Vector3>, IIterate<Vector3>
{
    public Triangle<Vector3> triangle;
    public Vector3 normal;
    public Vector3 uOffset;
    public Vector3 vOffset;
    public int Length => 3;
    public Vector3 this[int index] => triangle[index];
    public Vector3 V0 => triangle.v0;
    public Vector3 V1 => triangle.v1;
    public Vector3 V2 => triangle.v2;
    public GTriangle((Vector3 v0,Vector3 v1,Vector3 v2) _tuple) : this(_tuple.v0,_tuple.v1,_tuple.v2)
    {
    }

    public GTriangle(Vector3 _vertex0, Vector3 _vertex1, Vector3 _vertex2)
    {
        triangle = new Triangle<Vector3>(_vertex0, _vertex1, _vertex2);
        uOffset = _vertex1-_vertex0;
        vOffset = _vertex2-_vertex0;
        normal= Vector3.Cross(uOffset,vOffset);
    }
    public Vector3 GetUVPoint(float u,float v)=>(1f - u - v) * this[0] + u * uOffset + v * vOffset;

    public static GTriangle operator +(GTriangle _src, Vector3 _dst)=> new GTriangle(_src.V0 + _dst, _src.V1 + _dst, _src.V2 + _dst);
    public static GTriangle operator -(GTriangle _src, Vector3 _dst)=> new GTriangle(_src.V0 - _dst, _src.V1 - _dst, _src.V2 - _dst);
}
public interface IIterate<T>
    {
        T this[int _index] { get; }
        int Length { get; }
    }