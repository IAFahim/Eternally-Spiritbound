using Pancake;

namespace _Root.Scripts.Game.Guid
{
    public class GuidProvider: IGuidProvider
    {
        [Guid] public string guid;
        public string Guid => guid;
    }
}