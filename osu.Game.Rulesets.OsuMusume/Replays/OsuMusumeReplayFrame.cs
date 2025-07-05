// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.OsuMusume.Replays
{
    public class OsuMusumeReplayFrame : ReplayFrame
    {
        public List<OsuMusumeAction> Actions = new List<OsuMusumeAction>();

        public OsuMusumeReplayFrame(OsuMusumeAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }

        public override bool IsEquivalentTo(ReplayFrame other)
            => other is OsuMusumeReplayFrame scrollingFrame && Time == scrollingFrame.Time && Actions.SequenceEqual(scrollingFrame.Actions);
    }
}
