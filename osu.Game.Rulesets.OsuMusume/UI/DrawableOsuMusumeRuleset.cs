// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.OsuMusume.Objects;
using osu.Game.Rulesets.OsuMusume.Objects.Drawables;
using osu.Game.Rulesets.OsuMusume.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.OsuMusume.UI
{
    [Cached]
    public partial class DrawableOsuMusumeRuleset : DrawableScrollingRuleset<OsuMusumeHitObject>, IStartTimeProvider
    {
        public double StartTime { get; }

        public DrawableOsuMusumeRuleset(OsuMusumeRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 5000;
            VisualisationMethod = ScrollVisualisationMethod.Overlapping;

            if (beatmap.HitObjects.Count > 0)
            {
                StartTime = beatmap.HitObjects[0].StartTime;

                ControlPoints.AddRange([
                    new MultiplierControlPoint
                    {
                        Time = -100_000,
                        Velocity = 0.2f,
                    },
                    new MultiplierControlPoint
                    {
                        Time = beatmap.HitObjects[0].StartTime,
                        Velocity = 1,
                    }
                ]);
            }
        }

        protected override Playfield CreatePlayfield() => new OsuMusumePlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new OsuMusumeFramedReplayInputHandler(replay);

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new OsuMusumePlayfieldAdjustmentContainer();

        public override DrawableHitObject<OsuMusumeHitObject> CreateDrawableRepresentation(OsuMusumeHitObject h) => new DrawableOsuMusumeHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new OsuMusumeInputManager(Ruleset?.RulesetInfo);
    }
}
