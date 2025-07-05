// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.OsuMusume.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.OsuMusume.Replays
{
    public class OsuMusumeAutoGenerator : AutoGenerator<OsuMusumeReplayFrame>
    {
        public new Beatmap<OsuMusumeHitObject> Beatmap => (Beatmap<OsuMusumeHitObject>)base.Beatmap;

        public OsuMusumeAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new OsuMusumeReplayFrame());

            foreach (OsuMusumeHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new OsuMusumeReplayFrame
                {
                    Time = hitObject.StartTime
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
