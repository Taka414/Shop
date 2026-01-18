using VContainer;
using VContainer.Unity;

namespace Takap.Utility
{
    public class BgmAdjustmentLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SoundManager>();
            builder.RegisterComponentInHierarchy<PlayingTimeUpdater>();
            builder.RegisterComponentInHierarchy<PlayingLabelUpdater>();
        }
    }
}
