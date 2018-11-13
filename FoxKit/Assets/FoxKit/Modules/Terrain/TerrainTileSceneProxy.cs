using FoxKit.Modules.Terrain;

using UnityEditor;

using UnityEngine;

[ExecuteInEditMode]
public class TerrainTileSceneProxy : MonoBehaviour
{
    public Texture2D Heightmap;

    [SerializeField]
    private Mesh mesh;

    private const float Width = 128;

    public void Initialize(Mesh mesh, Material material, Texture2D heightmap, Texture2D weightmap)
    {
        // TODO Generate normals
        this.mesh = mesh;
        var meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = this.mesh;

        var materialClone = new Material(material);
        materialClone.SetTexture("_HeightTex", heightmap);
        materialClone.SetTexture("_MainTex", weightmap);
        materialClone.SetFloat("_MaxHeight", TerrainPreferences.Instance.MaxHeight);
        materialClone.SetFloat("_MinHeight", TerrainPreferences.Instance.MinHeight);

        var meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = materialClone;
    }
}
