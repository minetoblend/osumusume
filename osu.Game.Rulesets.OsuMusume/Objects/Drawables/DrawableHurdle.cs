using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.OsuMusume.UI;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables;

public partial class DrawableHurdle : DrawableOsuMusumeHitObject<Hurdle>, IKeyBindingHandler<OsuMusumeAction>
{
    public DrawableHurdle(Hurdle hitObject)
        : base(hitObject)
    {
        RelativeSizeAxes = Axes.Y;
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        for (int i = 7; i > 0; i--)
        {
            AddInternal(new Sprite
            {
                Texture = textures.Get("hurdle"),
                Origin = Anchor.BottomLeft,
                Y = i * OsuMusumePlayfield.ROW_HEIGHT,
                X = 3,
            });
        }
    }

    protected override void CheckForResult(bool userTriggered, double timeOffset)
    {
        if (!userTriggered)
        {
            if (!HitObject.HitWindows.CanBeHit(timeOffset))
                ApplyMinResult();

            return;
        }

        var result = HitObject.HitWindows.ResultFor(timeOffset);

        if (result == HitResult.None)
            return;

        ApplyResult(result);
    }

    public bool OnPressed(KeyBindingPressEvent<OsuMusumeAction> e)
    {
        if (Judged)
            return false;

        switch (e.Action)
        {
            case OsuMusumeAction.Jump:
                UpdateResult(true);
                return false;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<OsuMusumeAction> e)
    {
    }

    protected override void UpdateHitStateTransforms(ArmedState state)
    {
        switch (state)
        {
            case ArmedState.Miss:
                this.FadeColour(Color4.Red, 400)
                    .FadeOut(400);
                break;
        }
    }
}
