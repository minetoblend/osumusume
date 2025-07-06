using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.OsuMusume.Judgements;

public class SlideArrowJudgement : Judgement
{
    public override HitResult MinResult => HitResult.SmallTickMiss;

    public override HitResult MaxResult => HitResult.SmallTickHit;
}
