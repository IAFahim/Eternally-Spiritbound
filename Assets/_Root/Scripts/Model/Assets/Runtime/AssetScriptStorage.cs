using System;
using Pancake.Common;
using Sirenix.OdinInspector;
using Soul.Serializers.Runtime;
using Soul.Storages.Runtime;

namespace _Root.Scripts.Model.Assets.Runtime
{
    [Serializable]
    public class AssetScriptStorage : IntStorage<AssetScript>
    {
        private const string AppendKey = "_s";
        public override string StorageKey => $"{Guid}{AppendKey}";

        [Button]
        public void Load() => LoadData(Guid);

        [Button]
        public void Save() => SaveData();

        public override void LoadData(string guid)
        {
            Guid = guid;
            var datas = Data.Load(StorageKey, ToStringPair(DefaultData));
            SetData(ToGameItemPair(datas));
        }

        public override void SaveData(Pair<AssetScript, int>[] data)
        {
            Pair<string, int>[] stringIntData = ToStringPair(data);
            Data.Save(StorageKey, stringIntData);
        }

        public Pair<string, int>[] ToStringPair(Pair<AssetScript, int>[] datas)
        {
            var result = new Pair<string, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<string, int>(data.Key, data.Value);
            }

            return result;
        }

        public Pair<AssetScript, int>[] ToGameItemPair(Pair<string, int>[] datas)
        {
            var result = new Pair<AssetScript, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<AssetScript, int>(AssetScriptDataBase.Instance[data.Key], data.Value);
            }

            return result;
        }

        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}