using System;
using System.Threading;
using osu.Framework.Bindables;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.OsuMusume.UI;

namespace osu.Game.Rulesets.OsuMusume.Objects;

public class Slide : OsuMusumeHitObject, IHasDuration
{
    public readonly BindableList<SlidePosition> Nodes = new BindableList<SlidePosition>();

    public double SpanDuration { get; private set; }

    protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
    {
        base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

        SpanDuration = controlPointInfo.TimingPointAt(StartTime).BeatLength / 4;
    }

    protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
    {
        base.CreateNestedHitObjects(cancellationToken);

        if (Nodes.Count == 0)
            return;

        double time = StartTime;

        while (Precision.DefinitelyBigger(EndTime, time))
        {
            for (int i = 1; i < Nodes.Count; i++)
            {
                if (Nodes[i].StartTime >= time)
                {
                    var current = Nodes[i - 1];
                    var next = Nodes[i];

                    double progress = (time - current.StartTime) / (next.StartTime - current.StartTime);

                    float row = (float)Interpolation.Lerp(current.Row, next.Row, progress);

                    AddNested(new SlideArrow
                    {
                        StartTime = time,
                        Y = row * OsuMusumePlayfield.ROW_HEIGHT,
                    });

                    break;
                }
            }

            time += SpanDuration;
        }
    }

    public double EndTime => Nodes.Count == 0 ? StartTime : Nodes[^1].StartTime;

    public double Duration
    {
        get => EndTime - StartTime;
        set => throw new InvalidOperationException("Cannot change duration of slide");
    }
}

public record SlidePosition(double StartTime, int Row);
