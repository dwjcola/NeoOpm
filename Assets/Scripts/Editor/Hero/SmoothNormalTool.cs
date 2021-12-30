using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;


//-----------TA Tools by Yang---
//注意事项 非蒙皮模型新法线信息存储在顶点颜色 如果要用顶点色来做其他用途 需另行处理
//蒙皮模型只能存储在tagents里 如果蒙皮模型要使用切线相关关效果 需要另行处理

public class SmoothNormalTool
{
#if UNITY_EDITOR
    public enum NormalSaveType
    {
        Color,
        Tangent,
        UV4
    }

    //// [MenuItem("Tools/勾线平滑处理(切线)")]
    //[MenuItem("Assets/H3D/勾线平滑处理(切线)")]
    //static void SmothNormal2Tangent()
    //{
    //    DoNormalSmooth(NormalSaveType.Tangent);
    //}

    //// [MenuItem("Tools/勾线平滑处理(顶点色)")]
    //[MenuItem("Assets/H3D/勾线平滑处理(顶点色)")]
    //static void SmothNormal2Color()
    //{
    //    DoNormalSmooth(NormalSaveType.Color);
    //}



 

    [MenuItem("Assets/H3D/勾线平滑处理(UV4)")]
    static void SmothNormal2UV4()
    {
        DoNormalSmooth(NormalSaveType.UV4);
    }

    public static void DoNormalSmooth(NormalSaveType normalSaveType)
    {
        int ct = 0;
        foreach (GameObject go in Selection.gameObjects)
        {
            SkinnedMeshRenderer[] sks = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (normalSaveType == NormalSaveType.UV4)
            {
                ModelImporter modelImporter = null;
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                if (path.EndsWith(".fbx", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    modelImporter = ModelImporter.GetAtPath(path) as ModelImporter;
                }
                if (modelImporter != null)
                {
                    if (modelImporter.importTangents == ModelImporterTangents.None)
                    {
                        EditorUtility.DisplayDialog("Err", "FBX模型需要导入切线才能正确进行勾线平滑处理(UV4)", "OK");
                    }
                    /*
                    modelImporter.importNormals = ModelImporterNormals.Import;
                    modelImporter.importTangents = ModelImporterTangents.Import;
                    modelImporter.SaveAndReimport();
                    */
                }
            }

            for (int i = 0; i < sks.Length; i++)
            {
                SkinnedMeshRenderer sk = sks[i];
                if (sk.sharedMesh != null)
                {
                    Mesh originalMesh = sk.sharedMesh;
                    Mesh mesh = CloneMesh(sk.sharedMesh, sk.name + "_smooth");
                    Vector3[] normals = new List<Vector3>(mesh.normals).ToArray();
                    switch (normalSaveType)
                    {
                        case NormalSaveType.Color:
                            mesh.RecalculateNormals();
                            mesh.colors = DoAverageNormalsAsColors(mesh.normals, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                        case NormalSaveType.Tangent:
                            mesh.RecalculateNormals();
                            mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                        case NormalSaveType.UV4:
                            if (mesh.tangents.Length != mesh.vertexCount)
                            {
                                EditorUtility.DisplayDialog("Err", "FBX模型需要导入切线才能正确进行勾线平滑处理(UV4)", "OK");
                                continue;
                            }
                            mesh.RecalculateNormals();
                            mesh.uv4 = DoAverageNormalsAsUV(mesh.normals, normals, mesh.tangents, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                    }

                    string fpath = (System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(sk.sharedMesh)).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(sk.sharedMesh)), ""));
                    //save
                    AssetDatabase.CreateAsset(mesh, fpath + "/" + sk.name + "_mesh_smooth.asset");

                    sk.sharedMesh = mesh;
                    EditorUtility.SetDirty(go);

                    ct++;
                    EditorUtility.DisplayProgressBar("勾线处理", "勾线中 " + go.name, 0.4f);
                }
            }

            MeshFilter[] mfs = go.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter mf in mfs)
            {
                if (mf.sharedMesh != null)
                {
                    Mesh mesh = CloneMesh(mf.sharedMesh, mf.name + "_smooth");
                    Vector3[] normals = new List<Vector3>(mesh.normals).ToArray();
                    switch (normalSaveType)
                    {
                        case NormalSaveType.Color:
                            mesh.RecalculateNormals();
                            mesh.colors = DoAverageNormalsAsColors(mesh.normals, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                        case NormalSaveType.Tangent:
                            mesh.RecalculateNormals();
                            mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                        case NormalSaveType.UV4:
                            if (mesh.tangents.Length != mesh.vertexCount)
                            {
                                EditorUtility.DisplayDialog("Err", "FBX模型需要导入切线才能正确进行勾线平滑处理(UV4)", "OK");
                                continue;
                            }
                            mesh.RecalculateNormals();
                            mesh.uv4 = DoAverageNormalsAsUV(mesh.normals, normals, mesh.tangents, CreateGroups(mesh.vertices));
                            mesh.normals = normals;
                            break;
                    }

                    string fpath = (System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(mf.sharedMesh)).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(mf.sharedMesh)), ""));
                    //save
                    AssetDatabase.CreateAsset(mesh, fpath + "/" + mf.name + "_mesh_smooth.asset");

                    mf.sharedMesh = mesh;

                    EditorUtility.SetDirty(go);

                    ct++;
                    EditorUtility.DisplayProgressBar("勾线处理", "勾线中 " + go.name, 0.8f);
                }
            }
        }

        if (ct > 0)
        {
            EditorUtility.DisplayProgressBar("勾线处理", "保存中", 0.9f);
            AssetDatabase.SaveAssets();
            SceneView.RepaintAll();
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("勾线处理", "勾线处理完成,共" + ct + "个!", "Ok");
        }
    }

    public static void DoSmooth(GameObject go, NormalSaveType saveType)
    {
        if (saveType == NormalSaveType.UV4)
        {
            ModelImporter modelImporter = null;
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
            if (path.EndsWith(".fbx", System.StringComparison.CurrentCultureIgnoreCase))
            {
                modelImporter = ModelImporter.GetAtPath(path) as ModelImporter;
            }
            if (modelImporter != null)
            {
                if (modelImporter.importTangents == ModelImporterTangents.None)
                {
                    EditorUtility.DisplayDialog("Err", "FBX模型需要导入切线才能正确进行勾线平滑处理(UV4)", "OK");
                }
                /*
                modelImporter.importNormals = ModelImporterNormals.Import;
                modelImporter.importTangents = ModelImporterTangents.Import;
                modelImporter.SaveAndReimport();
                */
            }
        }

        SkinnedMeshRenderer[] sks = go.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var sk in sks)
        {
            var mesh = CreateSmoothMesh(sk.sharedMesh, sk.name, saveType, sk);
            if (mesh == null)
                continue;
            //sk.sharedMesh = mesh;
        }

        MeshFilter[] mfs = go.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            var mesh = CreateSmoothMesh(mf.sharedMesh, mf.name, saveType);
            if (mesh == null)
                continue;
            mf.sharedMesh = mesh;
        }
    }

    static Mesh CreateSmoothMesh(Mesh sharedMesh, string name, NormalSaveType saveType, SkinnedMeshRenderer smr = null)
    {
        if (sharedMesh == null)
        {
            UnityEngine.Debug.LogError("mesh引用为空，无法创建新mesh！");
            return null;
        }

        Mesh mesh = Object.Instantiate(sharedMesh);
        mesh.name = name + "_smooth";
        Vector3[] normals = new List<Vector3>(mesh.normals).ToArray();
        mesh.RecalculateNormals();
        switch (saveType)
        {
            case NormalSaveType.Color:
                mesh.colors = DoAverageNormalsAsColors(mesh.normals, CreateGroups(mesh.vertices));
                break;
            case NormalSaveType.Tangent:
                mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices));
                break;
            case NormalSaveType.UV4:
                if (mesh.tangents.Length != mesh.vertexCount)
                {
                    EditorUtility.DisplayDialog("Err", "FBX模型需要导入切线才能正确进行勾线平滑处理(UV4)", "OK");
                    return null;
                }
                mesh.uv4 = DoAverageNormalsAsUV(mesh.normals, normals, mesh.tangents, CreateGroups(mesh.vertices));
                break;
        }
        mesh.normals = normals;

        // Debug.Log("SMRBones=" + smr.bones.Length.ToString());
        // Debug.Log("BoneWeight=" + mesh.boneWeights.Length.ToString());
        // Debug.Log("BindPoses=" + mesh.bindposes.Length.ToString());

        string fpath = (System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(sharedMesh)).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(sharedMesh)), ""));
        string smoothMeshPath = fpath + "/" + name + "_mesh_smooth.asset";
        AssetDatabase.CreateAsset(mesh, smoothMeshPath);
        return mesh;
    }

    public static void DoNormalSmoothItem(GameObject obj, bool isSetTangent = false)
    {
        SkinnedMeshRenderer[] sks = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer sk in sks)
        {
            if (sk.sharedMesh != null)
            {
                Mesh mesh = CloneMesh(sk.sharedMesh, sk.name + "_smooth");

                //set to tagents
                mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices)); ;

                string fpath = (System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(sk.sharedMesh)).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(sk.sharedMesh)), ""));
                //save
                AssetDatabase.CreateAsset(mesh, fpath + "/" + sk.name + "_mesh_smooth.asset");

                sk.sharedMesh = mesh;

                EditorUtility.SetDirty(obj);
                SceneView.RepaintAll();
            }
        }

        MeshFilter[] mfs = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (mf.sharedMesh != null)
            {

                Mesh mesh = CloneMesh(mf.sharedMesh, mf.name + "_smooth");

                if (isSetTangent)
                {
                    mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices));
                }
                else
                {
                    mesh.colors = DoAverageNormalsAsColors(mesh.normals, CreateGroups(mesh.vertices));
                }
                string fpath = (System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(mf.sharedMesh)).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(mf.sharedMesh)), ""));
                //save
                AssetDatabase.CreateAsset(mesh, fpath + "/" + mf.name + "_mesh_smooth.asset");

                mf.mesh = mesh;

                EditorUtility.SetDirty(obj);
                SceneView.RepaintAll();
            }
        }
    }


    //资源自动处理
    public static void AutoMeshNormal(GameObject o, string path)
    {
        int ct = 0;
        string mes = "";

        SkinnedMeshRenderer[] sks = o.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer sk in sks)
        {
            if (sk.sharedMesh != null)
            {
                Mesh mesh = CloneMesh(sk.sharedMesh, sk.name + "_smooth");

                sk.sharedMaterial.shader = Shader.Find("GuYin/Toon_Outline_MatCap_Smooth");

                //set to tagents
                mesh.tangents = DoAverageNormalsAsTangents(mesh.normals, CreateGroups(mesh.vertices));

                string fpath = (System.IO.Path.GetDirectoryName(path).Replace(System.IO.Path.GetFileName(path), ""));
                //save
                AssetDatabase.CreateAsset(mesh, fpath + "/" + sk.name + "_mesh_smooth.asset");

                mes += "自动平滑处理:" + fpath + "/" + sk.name + "_mesh_smooth.asset." + "\n";

                sk.sharedMesh = mesh;

                EditorUtility.SetDirty(o);
                SceneView.RepaintAll();

                ct++;
            }
        }

        MeshFilter[] mfs = o.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (mf.sharedMesh != null)
            {
                mf.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("GuYin/Toon_Outline_Test_Smooth");

                Mesh mesh = CloneMesh(mf.sharedMesh, mf.name + "_smooth");

                mesh.colors = DoAverageNormalsAsColors(mesh.normals, CreateGroups(mesh.vertices));

                string fpath = (System.IO.Path.GetDirectoryName(path).Replace(System.IO.Path.GetFileName(path), ""));

                //save
                AssetDatabase.CreateAsset(mesh, fpath + "/" + mf.name + "_mesh_smooth.asset");

                mes += "自动生成平滑模型:" + fpath + "/" + mf.name + "_mesh_smooth.asset." + "\n";

                mf.mesh = mesh;

                EditorUtility.SetDirty(o);
                SceneView.RepaintAll();

                ct++;
            }
        }

        if (ct > 0)
        {
            Debug.Log(mes);
        }
    }

#endif

    public static List<List<int>> CreateGroups(Vector3[] vertices)
    {
        List<List<int>> verticesGroups = new List<List<int>>();
        for (int i = 0; i < vertices.Length; i++)
        {
            bool alone = true;
            foreach (List<int> verticesGroup in verticesGroups)
            {
                if (vertices[verticesGroup[0]] == vertices[i])
                {
                    verticesGroup.Add(i);
                    alone = false;
                    break;
                }
            }

            if (alone)
            {
                List<int> list = new List<int>();
                list.Add(i);
                verticesGroups.Add(list);
            }
        }

        return verticesGroups;
    }

    static Color[] DoAverageNormalsAsColors(Vector3[] normals, List<List<int>> groups)
    {
        Color[] colors = new Color[normals.Length];
        Vector3[] meshNormals = DoAverageNormals(normals, groups);
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(meshNormals[i].x, meshNormals[i].y, meshNormals[i].z);
        }
        return colors;
    }

    static Vector4[] DoAverageNormalsAsTangents(Vector3[] normals, List<List<int>> groups)
    {
        Vector4[] tangents = new Vector4[normals.Length];
        Vector3[] meshNormals = DoAverageNormals(normals, groups);
        for (int i = 0; i < tangents.Length; i++)
        {
            tangents[i] = new Vector4(meshNormals[i].x, meshNormals[i].y, meshNormals[i].z, 0f);
        }
        return tangents;
    }

    public static Vector2[] DoAverageNormalsAsUV(Vector3[] needSaveNormal, Vector3[] normals, Vector4[] tangents, List<List<int>> groups)
    {
        Vector2[] uv = new Vector2[normals.Length];
        Vector3[] meshNormals = DoAverageNormals(needSaveNormal, groups);
        for (int i = 0; i < uv.Length; i++)
        {
            Vector3 tangent = new Vector3(tangents[i].x, tangents[i].y, tangents[i].z);
            Vector3 binormal = Vector3.Cross(normals[i], tangent) * tangents[i].w;
            Vector3 smoothNormalInTangent = new Vector3(Vector3.Dot(tangent, meshNormals[i]),
                                                        Vector3.Dot(binormal, meshNormals[i]),
                                                        Vector3.Dot(normals[i], meshNormals[i]));
            uv[i] = new Vector2(smoothNormalInTangent.x, smoothNormalInTangent.y);
        }
        return uv;
    }

    static Vector3[] DoAverageNormals(Vector3[] normals, List<List<int>> groups)
    {
        Vector3[] averageNormals = new Vector3[normals.Length];
        foreach (List<int> group in groups)
        {
            Vector3 average = Vector3.zero;
            foreach (int i in group)
            {
                average += normals[i];
            }
            average.Normalize();

            foreach (int i in group)
            {
                averageNormals[i] = average;
            }
        }

        return averageNormals;
    }

   public static Mesh CloneMesh(Mesh originalMesh, string cloneMeshName)
    {
        Mesh newMesh = new Mesh
        {
            name = cloneMeshName,
            vertices = originalMesh.vertices,
            normals = originalMesh.normals,
            tangents = originalMesh.tangents,
            uv = originalMesh.uv,
            uv2 = originalMesh.uv2,
            uv3 = originalMesh.uv3,
            uv4 = originalMesh.uv4,
            colors32 = originalMesh.colors32,
            triangles = originalMesh.triangles,
            bindposes = originalMesh.bindposes,
            boneWeights = originalMesh.boneWeights
        };

        //Only available from Unity 5.3 onward
        if (originalMesh.blendShapeCount > 0)
            CopyBlendShapes(originalMesh, newMesh);

        newMesh.subMeshCount = originalMesh.subMeshCount;
        if (newMesh.subMeshCount > 1)
        {
            for (var i = 0; i < newMesh.subMeshCount; i++)
            {
                newMesh.SetTriangles(originalMesh.GetTriangles(i), i);
            }
        }

        return newMesh;
    }

    static void CopyBlendShapes(Mesh originalMesh, Mesh newMesh)
    {
        for (var i = 0; i < originalMesh.blendShapeCount; i++)
        {
            var shapeName = originalMesh.GetBlendShapeName(i);
            var frameCount = originalMesh.GetBlendShapeFrameCount(i);
            for (var j = 0; j < frameCount; j++)
            {
                var dv = new Vector3[originalMesh.vertexCount];
                var dn = new Vector3[originalMesh.vertexCount];
                var dt = new Vector3[originalMesh.vertexCount];

                var frameWeight = originalMesh.GetBlendShapeFrameWeight(i, j);
                originalMesh.GetBlendShapeFrameVertices(i, j, dv, dn, dt);
                newMesh.AddBlendShapeFrame(shapeName, frameWeight, dv, dn, dt);
            }
        }
    }
}
