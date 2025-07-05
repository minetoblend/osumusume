using osu.Framework.Allocation;

namespace osu.Game.Rulesets.OsuMusume.UI;

[Cached]
public interface IStartTimeProvider
{
    public double StartTime { get; }
}
