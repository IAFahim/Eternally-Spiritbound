using System;
using Pancake.Common;
using Soul.Serializers.Runtime;
using Soul.Storages.Runtime;

namespace _Root.Scripts.Game.Items.Runtime.Storage
{
    [Serializable]
    public class ItemStorage : IntStorage<ItemBase>
    {
        public AllGameItem allGameItem;
        public string appendKey = "_string_int";
        public override string StorageKey => $"{Guid}{appendKey}";
        
        public void Load()
        {
            
        }
        
        public override void LoadData(string guid)
        {
            Guid = guid;
            var datas= Data.Load(StorageKey,ToStringPair(DefaultData));
            SetData(ToGameItemPair(datas));
        }

        public override void SaveData(Pair<ItemBase, int>[] data)
        {
            Pair<string, int>[] stringIntData = ToStringPair(data);
            Data.Save(StorageKey, stringIntData);
        }

        public Pair<string, int>[] ToStringPair(Pair<ItemBase, int>[] datas)
        {
            var result = new Pair<string, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<string, int>(data.Key, data.Value);
            }

            return result;
        }

        public Pair<ItemBase, int>[] ToGameItemPair(Pair<string, int>[] datas)
        {
            var result = new Pair<ItemBase, int>[datas.Length];
            for (var i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                result[i] = new Pair<ItemBase, int>(allGameItem[data.Key], data.Value);
            }

            return result;
        }

        public override void ClearStorage() => Data.DeleteKey(StorageKey);
    }
}