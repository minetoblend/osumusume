using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.OsuMusume.UI;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables;

public partial class DrawableSlideArrow : DrawableOsuMusumeHitObject<SlideArrow>
{
    public DrawableSlideArrow(SlideArrow hitObject)
        : base(hitObject)
    {
    }

    private Sprite arrow;
    private Sprite flash;
    private Container content;

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        RelativeSizeAxes = Axes.Both;

        AddInternal(content = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Scale = new Vector2(1, 0.75f),
            Children =
            [
                arrow = new Sprite
                {
                    RelativePositionAxes = Axes.X,
                    Texture = textures.Get("slide_arrow"),
                    Origin = Anchor.Centre,
                },
                flash = new Sprite
                {
                    RelativePositionAxes = Axes.X,
                    Texture = textures.Get("slide_arrow"),
                    Origin = Anchor.Centre,
                    Blending = BlendingParameters.Additive,
                    Alpha = 0,
                }
            ]
        });
    }

    protected override void UpdateHitStateTransforms(ArmedState state)
    {
        this.Delay(400).FadeOut();

        switch (state)
        {
            case ArmedState.Hit:
                arrow.FadeOut();
                flash.FadeIn()
                     .FadeOut(400, Easing.Out)
                     .ScaleTo(1.25f, 400, Easing.OutExpo);

                break;

            default:
                this.FadeOut(400);

                break;
        }
    }

    protected override void OnApply()
    {
        base.OnApply();

        var parent = (Slide)ParentHitObject.HitObject;

        content.X = (float)((HitObject.StartTime - parent.StartTime) / parent.Duration);
        content.Y = HitObject.Y + OsuMusumePlayfield.ROW_HEIGHT / 2;
    }

    [Resolved]
    private PlayerUma player { get; set; }

    protected override void CheckForResult(bool userTriggered, double timeOffset)
    {
        if (timeOffset >= 0)
        {
            float distance = Math.Abs(player.YPosition - HitObject.Y);

            if (distance < 20)
                ApplyMaxResult();
            else
                ApplyMinResult();
        }
    }
}
