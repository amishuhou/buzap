using Caliburn.Micro;

namespace UsedParts.UI.Tombstoning
{
    public abstract class ScreenStorageHandler<T> : StorageHandler<T>
        where T : IScreen
    {
        public override void Configure()
        {
            Id(e => e.DisplayName);
        }
    }
}
