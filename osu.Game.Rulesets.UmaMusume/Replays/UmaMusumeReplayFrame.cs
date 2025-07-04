// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.UmaMusume.Replays
{
    public class UmaMusumeReplayFrame : ReplayFrame
    {
        public List<UmaMusumeAction> Actions = new List<UmaMusumeAction>();

        public UmaMusumeReplayFrame(UmaMusumeAction? button = null)
        {
            if (button.HasValue)
                Actions.Add(button.Value);
        }

        public override bool IsEquivalentTo(ReplayFrame other)
            => other is UmaMusumeReplayFrame scrollingFrame && Time == scrollingFrame.Time && Actions.SequenceEqual(scrollingFrame.Actions);
    }
}
