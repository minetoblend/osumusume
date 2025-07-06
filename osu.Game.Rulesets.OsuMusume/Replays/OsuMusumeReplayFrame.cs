// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Replays.Legacy;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Types;

namespace osu.Game.Rulesets.OsuMusume.Replays
{
    public class OsuMusumeReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public List<OsuMusumeAction> Actions = new List<OsuMusumeAction>();

        public OsuMusumeReplayFrame(OsuMusumeAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }

        public override bool IsEquivalentTo(ReplayFrame other)
            => other is OsuMusumeReplayFrame scrollingFrame && Time == scrollingFrame.Time && Actions.SequenceEqual(scrollingFrame.Actions);

        public void FromLegacy(LegacyReplayFrame currentFrame, IBeatmap beatmap, ReplayFrame lastFrame = null)
        {
            if (currentFrame.MouseLeft1) Actions.Add(OsuMusumeAction.Hit1);
            if (currentFrame.MouseRight1) Actions.Add(OsuMusumeAction.Hit2);
            if (currentFrame.MouseLeft2) Actions.Add(OsuMusumeAction.Up);
            if (currentFrame.MouseRight2) Actions.Add(OsuMusumeAction.Down);
            if (currentFrame.Smoke) Actions.Add(OsuMusumeAction.Jump);
        }

        public LegacyReplayFrame ToLegacy(IBeatmap beatmap)
        {
            ReplayButtonState state = ReplayButtonState.None;

            if (Actions.Contains(OsuMusumeAction.Hit1))
                state |= ReplayButtonState.Left1;
            if (Actions.Contains(OsuMusumeAction.Hit2))
                state |= ReplayButtonState.Right1;
            if (Actions.Contains(OsuMusumeAction.Up))
                state |= ReplayButtonState.Left2;
            if (Actions.Contains(OsuMusumeAction.Down))
                state |= ReplayButtonState.Right2;
            if (Actions.Contains(OsuMusumeAction.Jump))
                state |= ReplayButtonState.Smoke;

            return new LegacyReplayFrame(Time, null, null, state);
        }
    }
}
