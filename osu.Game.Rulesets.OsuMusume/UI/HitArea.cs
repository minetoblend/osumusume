using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class HitArea : CompositeDrawable
{
    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    [Resolved(CanBeNull = true)]
    private IStartTimeProvider startTimeProvider { get; set; }

    public HitArea()
    {
        Origin = Anchor.TopCentre;
        Width = 20;
        RelativeSizeAxes = Axes.Y;
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        InternalChild = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Alpha = 0.05f,
            Blending = BlendingParameters.Additive,
        };
    }

    protected override void Update()
    {
        base.Update();

        double time = Math.Max(startTimeProvider?.StartTime - 1 ?? Time.Current, Time.Current);

        X = scrollingInfo.Algorithm.Value.PositionAt(time, Time.Current, scrollingInfo.TimeRange.Value, DrawWidth);
    }
}
