using _Root.Scripts.Model.Assets.Runtime;
using _Root.Scripts.Model.Plants.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = " Seed", menuName = "Scriptable/Asset/Plants/Seeds")]
public class SeedAsset : AssetScript
{
    public Material[] materials;
    public Mesh[] meshes;
    
    public PlantAsset plant;
}
