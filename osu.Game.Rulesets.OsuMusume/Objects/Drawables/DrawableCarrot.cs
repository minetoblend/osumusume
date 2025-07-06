using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.OsuMusume.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables;

public partial class DrawableCarrot : DrawableOsuMusumeHitObject<Carrot>
{
    private readonly Container content;

    [Resolved]
    private OsuMusumePlayfield playfield { get; set; }

    public DrawableCarrot(Carrot hitObject)
        : base(hitObject)
    {
        Size = new Vector2(24);
        Origin = Anchor.BottomCentre;

        Y = (hitObject.Row + 0.5f) * OsuMusumePlayfield.ROW_HEIGHT;

        AddInternal(content = new Container
        {
            RelativeSizeAxes = Axes.Both,
        });
    }

    private Sprite shadow;
    private Drawable shadowProxy;
    private Sprite carrot;
    private Drawable flash;

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        content.AddRange([
            shadow = new Sprite
            {
                Texture = textures.Get("shadow"),
                Scale = new Vector2(0.35f, 0.25f),
                Colour = Color4.DarkBlue,
                Alpha = 0.125f,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
            },
            flash = new Container
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(20),
                Alpha = 0,
                Blending = BlendingParameters.Additive,
                Child = new Circle
                {
                    RelativeSizeAxes = Axes.Both,
                    Scale = new Vector2(1, 0.5f),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                }
            },
            new FloatingContainer
            {
                RelativeSizeAxes = Axes.Both,
                StartTime = HitObject.StartTime,
                Children =
                [
                    carrot = new Sprite
                    {
                        Texture = textures.Get("carrot"),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                ]
            }
        ]);

        playfield.ShadowLayer.Add(shadowProxy = shadow.CreateProxy());
    }

    protected override void CheckForResult(bool userTriggered, double timeOffset)
    {
        if (timeOffset >= 0)
            // todo: implement judgement logic
            ApplyMaxResult();
    }

    protected override void UpdateHitStateTransforms(ArmedState state)
    {
        const double duration = 1000;

        switch (state)
        {
            case ArmedState.Hit:
                carrot.ScaleTo(1.2f, 200, Easing.OutExpo)
                      .FadeOut(200, Easing.Out);
                flash.FadeTo(0.25f)
                     .FadeOut(400, Easing.Out)
                     .ScaleTo(1.5f, 400, Easing.OutExpo);
                shadow.ScaleTo(0, 200, Easing.Out);
                break;

            case ArmedState.Miss:

                this.FadeColour(Color4.Red, duration);
                this.FadeOut(duration, Easing.InQuint);
                break;
        }

        this.Delay(1000).FadeOut().Expire();
    }
}
