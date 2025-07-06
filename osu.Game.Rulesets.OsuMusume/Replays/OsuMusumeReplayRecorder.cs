using System.Collections.Generic;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.Replays;

public partial class OsuMusumeReplayRecorder : ReplayRecorder<OsuMusumeAction>
{
    public OsuMusumeReplayRecorder(Score target)
        : base(target)
    {
    }

    protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<OsuMusumeAction> actions, ReplayFrame previousFrame)
        => new OsuMusumeReplayFrame { Time = Time.Current, Actions = actions };
}
