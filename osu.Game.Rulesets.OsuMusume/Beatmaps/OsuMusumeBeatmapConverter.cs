// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.OsuMusume.Objects;

namespace osu.Game.Rulesets.OsuMusume.Beatmaps
{
    public class OsuMusumeBeatmapConverter : BeatmapConverter<OsuMusumeHitObject>
    {
        private readonly Random random;
        private int lastRow;

        public OsuMusumeBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
            random = new Random(0);

            lastRow = random.Next(6);
        }

        public override bool CanConvert() => true;

        protected override IEnumerable<OsuMusumeHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            int row;

            do
            {
                row = lastRow + random.Next(-1, 2);
            } while (row < 0 || row >= 6);

            yield return new OsuMusumeHitObject
            {
                Samples = original.Samples,
                StartTime = original.StartTime,
                Row = row,
            };

            lastRow = row;
        }
    }
}
