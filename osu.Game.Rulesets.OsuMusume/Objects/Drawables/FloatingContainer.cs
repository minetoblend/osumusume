using System;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables;

public partial class FloatingContainer : Container
{
    public double StartTime { get; set; }

    protected override void Update()
    {
        base.Update();

        Y = MathF.Sin((float)(Time.Current - StartTime) / 250);
    }
}
