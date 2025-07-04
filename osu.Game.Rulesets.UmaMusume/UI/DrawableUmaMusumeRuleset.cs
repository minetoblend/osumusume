// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UmaMusume.Objects;
using osu.Game.Rulesets.UmaMusume.Objects.Drawables;
using osu.Game.Rulesets.UmaMusume.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.UmaMusume.UI
{
    [Cached]
    public partial class DrawableUmaMusumeRuleset : DrawableScrollingRuleset<UmaMusumeHitObject>
    {
        public DrawableUmaMusumeRuleset(UmaMusumeRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
            Direction.Value = ScrollingDirection.Left;
            TimeRange.Value = 6000;
        }

        protected override Playfield CreatePlayfield() => new UmaMusumePlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new UmaMusumeFramedReplayInputHandler(replay);

        public override DrawableHitObject<UmaMusumeHitObject> CreateDrawableRepresentation(UmaMusumeHitObject h) => new DrawableUmaMusumeHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new UmaMusumeInputManager(Ruleset?.RulesetInfo);
    }
}
