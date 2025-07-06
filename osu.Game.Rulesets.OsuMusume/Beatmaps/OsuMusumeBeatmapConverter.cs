// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.OsuMusume.Objects;

namespace osu.Game.Rulesets.OsuMusume.Beatmaps
{
    public class OsuMusumeBeatmapConverter : BeatmapConverter<OsuMusumeHitObject>
    {
        private readonly Random random;
        private int lastRow;
        private double lastStartTime;

        public OsuMusumeBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
            random = new Random(0);

            lastRow = random.Next(7);
        }

        public override bool CanConvert() => true;

        protected override IEnumerable<OsuMusumeHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            if (original is IHasPathWithRepeats withRepeats)
            {
                double spanDuration = withRepeats.Duration / withRepeats.SpanCount();

                var slide = new Slide
                {
                    StartTime = original.StartTime,
                };

                for (int i = 0; i <= withRepeats.SpanCount(); i++)
                {
                    var position = withRepeats.CurvePositionAt((float)i / withRepeats.SpanCount());

                    if (original is IHasPosition pos)
                        position += pos.Position;

                    int row = (int)(position.Y / 384 * 7);

                    slide.Nodes.Add(new SlidePosition(original.StartTime + i * spanDuration, row));
                }

                yield return slide;

                yield break;
            }

            if (original is IHasCombo combo && combo.NewCombo)
            {
                yield return new Hurdle
                {
                    Samples = original.Samples,
                    StartTime = original.StartTime,
                };

                yield break;
            }

            yield return new Carrot
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Row = original is IHasYPosition h ? (h.Y / 384f * 7) : nextRow(original.StartTime),
            };
        }

        private int nextRow(double startTime)
        {
            int row = startTime - lastStartTime < 100
                ? lastRow
                : lastRow switch
                {
                    0 => 1,
                    6 => 5,
                    _ => lastRow + (random.NextSingle() > 0.5 ? 1 : -1)
                };

            lastRow = row;
            lastStartTime = startTime;

            return row;
        }
    }
}
