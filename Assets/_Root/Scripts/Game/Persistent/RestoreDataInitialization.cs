using Pancake;

namespace _Root.Scripts.Game.Persistent
{
    [EditorIcon("icon_default")]
    public class RestoreDataInitialization : BaseInitialization
    {
        public override async void Init()
        {
            // You need to restore the previous data here before moving to another scene.

            // block loading scene completed before restoring data from server is complete.

            await BackupDataHelper.Restore();
            // todo
        }
    }
}