// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.UmaMusume.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.UmaMusume.Replays
{
    public class UmaMusumeAutoGenerator : AutoGenerator<UmaMusumeReplayFrame>
    {
        public new Beatmap<UmaMusumeHitObject> Beatmap => (Beatmap<UmaMusumeHitObject>)base.Beatmap;

        public UmaMusumeAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new UmaMusumeReplayFrame());

            foreach (UmaMusumeHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new UmaMusumeReplayFrame
                {
                    Time = hitObject.StartTime
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
