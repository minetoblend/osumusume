using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.OsuMusume.Judgements;

namespace osu.Game.Rulesets.OsuMusume.Objects;

public class SlideArrow : OsuMusumeHitObject, IHasYPosition
{
    public float Y { get; set; }

    public override Judgement CreateJudgement() => new SlideArrowJudgement();
}
