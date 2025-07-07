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
using osu.Game.Scoring;

namespace osu.Game.Rulesets.OsuMusume.UI
{
    [Cached]
    public partial class DrawableOsuMusumeRuleset : DrawableScrollingRuleset<OsuMusumeHitObject>, IStartTimeProvider
    {
        public double StartTime { get; private set; }

        public DrawableOsuMusumeRuleset(OsuMusumeRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (Beatmap.HitObjects.Count > 0)
            {
                StartTime = Beatmap.HitObjects[0].StartTime;

                ControlPoints.Clear();
                ControlPoints.AddRange([
                    new MultiplierControlPoint
                    {
                        Time = -100_000,
                        Velocity = 0.2f,
                    },
                    new MultiplierControlPoint
                    {
                        Time = StartTime,
                        Velocity = 1f,
                    }
                ]);

                foreach (var effectPoint in Beatmap.ControlPointInfo.EffectPoints)
                {
                    if (effectPoint.Time < StartTime)
                        continue;

                    ControlPoints.Add(new MultiplierControlPoint
                    {
                        Time = effectPoint.Time,
                        Velocity = effectPoint.KiaiMode ? 1.5f : 1
                    });
                }
            }

            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 1500;
            VisualisationMethod = ScrollVisualisationMethod.Sequential;
        }

        protected override Playfield CreatePlayfield() => new OsuMusumePlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new OsuMusumeFramedReplayInputHandler(replay);

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new OsuMusumePlayfieldAdjustmentContainer();

        public override DrawableHitObject<OsuMusumeHitObject> CreateDrawableRepresentation(OsuMusumeHitObject h) => h switch
        {
            Carrot carrot => new DrawableCarrot(carrot),
            Slide slide => new DrawableSlide(slide),
            Hurdle hurdle => new DrawableHurdle(hurdle),
            _ => null,
        };

        protected override PassThroughInputManager CreateInputManager() => new OsuMusumeInputManager(Ruleset?.RulesetInfo);

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new OsuMusumeReplayRecorder(score);
    }
}
