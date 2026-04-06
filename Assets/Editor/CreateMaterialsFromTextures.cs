using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateMaterialsFromTextures
{
    [MenuItem("Tools/Create Materials From Textures")]
    static void CreateMaterials()
    {
        string folder = "Assets/Textures"; // change to your folder path
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });
        if (guids.Length == 0) { Debug.Log("No textures found."); return; }

        string matFolder = folder + "/Materials";
        if (!AssetDatabase.IsValidFolder(matFolder)) AssetDatabase.CreateFolder(folder, "Materials");

        foreach (var g in guids)
        {
            string texPath = AssetDatabase.GUIDToAssetPath(g);
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            if (tex == null) continue;

            string matPath = Path.Combine(matFolder, Path.GetFileNameWithoutExtension(texPath) + ".mat");
            if (File.Exists(matPath)) continue;

            Material mat = new Material(Shader.Find("Standard")); // change shader if needed
            mat.mainTexture = tex;
            AssetDatabase.CreateAsset(mat, matPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Created {guids.Length} materials in {matFolder}");
    }
}
