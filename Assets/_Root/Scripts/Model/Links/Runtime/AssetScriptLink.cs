using _Root.Scripts.Model.Assets.Runtime;
using Soul.Links.Runtime;

namespace _Root.Scripts.Model.Links.Runtime
{
    public abstract class AssetScriptLink<T> : Link<AssetScript, T>
    {
        public override AssetScript GetSource(string source) => AssetScriptDataBase.Instance[source];
    }
}