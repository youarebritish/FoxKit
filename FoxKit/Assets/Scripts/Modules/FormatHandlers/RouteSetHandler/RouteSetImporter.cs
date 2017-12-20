using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.AssetImporters;

using UnityEngine;

//[ScriptedImporter(1, "frt")]
public class RouteSetImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var position = new Vector3(10, 42, 69);

        cube.transform.position = position;

        // 'cube' is a a GameObject and will be automatically converted into a prefab
        // (Only the 'Main Asset' is elligible to become a Prefab.)
        ctx.AddObjectToAsset("main obj", cube);
        ctx.SetMainObject(cube);

        var material = new Material(Shader.Find("Standard"));
        material.color = Color.red;

        // Assets must be assigned a unique identifier string consistent across imports
        ctx.AddObjectToAsset("my Material", material);

        // Assets that are not passed into the context as import outputs must be destroyed
        var tempMesh = new Mesh();
        DestroyImmediate(tempMesh);
    }
}