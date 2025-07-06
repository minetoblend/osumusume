using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables;

public partial class DrawableSlide : DrawableOsuMusumeHitObject<Slide>
{
    public DrawableSlide(Slide hitObject)
        : base(hitObject)
    {
    }

    protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
    {
        if (hitObject is SlideArrow arrow)
        {
            return new DrawableSlideArrow(arrow);
        }

        return base.CreateNestedHitObject(hitObject);
    }

    private Container<DrawableSlideArrow> arrowContainer;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Y;

        AddInternal(arrowContainer = new Container<DrawableSlideArrow>
        {
            RelativePositionAxes = Axes.Both,
        });
    }

    protected override void AddNestedHitObject(DrawableHitObject hitObject)
    {
        if (hitObject is DrawableSlideArrow arrow)
        {
            arrow.Depth = (float)hitObject.HitObject.StartTime;
            arrowContainer.Add(arrow);
        }

        base.AddNestedHitObject(hitObject);
    }

    protected override void CheckForResult(bool userTriggered, double timeOffset)
    {
        if (timeOffset >= 0)
        {
            int numHits = 0;

            foreach (var nested in NestedHitObjects)
            {
                if (nested.IsHit)
                    numHits++;
            }

            float ratio = (float)numHits / NestedHitObjects.Count;

            if (ratio == 1)
                ApplyResult(HitResult.Perfect);
            else if (ratio > 0.9)
                ApplyResult(HitResult.Great);
            else if (ratio > 0.75)
                ApplyResult(HitResult.Ok);
            else if (ratio > 0.5)
                ApplyResult(HitResult.Meh);
            else
                ApplyResult(HitResult.Miss);
        }
    }

    protected override void UpdateHitStateTransforms(ArmedState state)
    {
        this.Delay(400).FadeOut();
    }
}
