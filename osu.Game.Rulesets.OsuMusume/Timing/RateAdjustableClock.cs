using osu.Framework.Timing;

namespace osu.Game.Rulesets.OsuMusume.Timing;

public class RateAdjustableClock(IFrameBasedClock source) : IFrameBasedClock
{
    public double CurrentTime { get; private set; }
    public double Rate { get; set; } = 1;

    public bool IsRunning => source.IsRunning;

    public double ElapsedFrameTime => source.ElapsedFrameTime * Rate;

    public double FramesPerSecond => source.FramesPerSecond;

    public void ProcessFrame()
    {
        CurrentTime += source.ElapsedFrameTime * Rate;
    }
}
