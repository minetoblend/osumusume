using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class DefaultOsuMusumeJudgementPiece : DefaultJudgementPiece
{
    public DefaultOsuMusumeJudgementPiece(HitResult result)
        : base(result)
    {
        Anchor = Anchor.Centre;
    }
}
