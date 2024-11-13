using _Root.Scripts.Model.Assets.Runtime;
using Pancake.Common;
using Soul.Links.Runtime;
using Soul.Serializers.Runtime;
using UnityEngine;

namespace _Root.Scripts.Model.Links.Runtime
{
    [CreateAssetMenu(menuName = "Scriptable/Link/AssetOwnAssetCountGlobal", fileName = "AssetOwnAssetCountGlobalLink")]
    public class AssetOwnAssetGlobalCountLink : Link<AssetScript, int>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];


        public void SaveAll()
        {
            Pair<string, int>[] pairs = new Pair<string, int>[Dictionary.Count];
            foreach (var pair in Dictionary) pairs[pair.Value] = new Pair<string, int>(pair.Key.Guid, pair.Value);
            Data.Save(Guid, pairs);
        }

        public void LoadAll()
        {
            Pair<string, int>[] pairs = Data.Load<Pair<string, int>[]>(Guid);
            foreach (var pair in pairs)
            {
                Dictionary.Add(GetSource(pair.Key), pair.Value);
            }
        }

        public void Add(AssetScript assetScriptReference, int amount)
        {
            if (!Dictionary.TryAdd(assetScriptReference, amount)) Dictionary[assetScriptReference] += amount;
        }

        public void Remove(AssetScript assetScriptReference, int amount)
        {
            if (Dictionary.ContainsKey(assetScriptReference))
            {
                Dictionary[assetScriptReference] -= amount;
                if (Dictionary[assetScriptReference] <= 0)
                {
                    Dictionary.Remove(assetScriptReference);
                }
            }
        }
    }
}