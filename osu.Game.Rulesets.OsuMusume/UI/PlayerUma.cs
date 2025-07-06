using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.OsuMusume.Graphics;
using osu.Game.Rulesets.OsuMusume.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class PlayerUma : CompositeDrawable, IUma, IKeyBindingHandler<OsuMusumeAction>
{
    private Vector2 targetPosition;
    private Vector2 velocity;

    private bool dashing;

    private Vector2 movementSpeed => new Vector2(0, dashing ? 0.25f : 0.15f);

    private readonly DrawableUma drawableUma;

    public float YPosition => targetPosition.Y;

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    [Resolved]
    private IStartTimeProvider startTimeProvider { get; set; }

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    private readonly Container content;
    private readonly Box rowHighlight;

    public PlayerUma(UmaType type)
    {
        RelativeSizeAxes = Axes.Both;

        InternalChildren =
        [
            rowHighlight = new Box
            {
                Width = 10000,
                Height = OsuMusumePlayfield.ROW_HEIGHT,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Alpha = 0.1f,
                Blending = BlendingParameters.Additive,
            },
            content = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children =
                [
                    drawableUma = new DrawableUma(type)
                ],
            }
        ];
    }

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        content.Add(new FloatingContainer
        {
            new Sprite
            {
                Texture = textures.Get("player_marker"),
                Origin = Anchor.BottomCentre,
                Y = -35,
            }
        });
    }

    [Resolved]
    private OsuMusumePlayfield playfield { get; set; }

    protected override void LoadComplete()
    {
        base.LoadComplete();
        playfield.BackgroundLayer.Add(rowHighlight.CreateProxy());
    }

    protected override void Update()
    {
        base.Update();

        float rate = scrollAlgorithm.GetLength(Time.Current, Time.Current + 30, timeRange, 100);

        drawableUma.AnimationRate = Time.Current > startTimeProvider.StartTime ? float.Clamp(rate, 0.5f, 1.5f) : 0;

        if (Time.Current > startTimeProvider.StartTime)
        {
            targetPosition = Vector2.Clamp(targetPosition + velocity * movementSpeed * (float)Time.Elapsed, Vector2.Zero, new Vector2(64, 130));

            Position = Vector2.Lerp(targetPosition, Position, (float)Math.Exp(-0.03f * Time.Elapsed));
        }
    }

    private void jump()
    {
        content.FinishTransforms();
        content.MoveToY(-30, 200, Easing.OutCubic)
               .Then()
               .MoveToY(0, 400, Easing.InCubic);
    }

    private void endJump()
    {
        content.ClearTransforms();
        content.MoveToY(0, 200, Easing.In);
    }

    public bool OnPressed(KeyBindingPressEvent<OsuMusumeAction> e)
    {
        switch (e.Action)
        {
            case OsuMusumeAction.Dash:
                dashing = true;
                break;

            case OsuMusumeAction.Down:
                velocity.Y += 1;
                break;

            case OsuMusumeAction.Up:
                velocity.Y -= 1;
                break;

            case OsuMusumeAction.Jump:
                jump();
                break;

            case OsuMusumeAction.Hit1:
            case OsuMusumeAction.Hit2:
                rowHighlight.FadeTo(0.5f)
                            .FadeTo(0.1f, 300, Easing.OutCubic);
                break;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<OsuMusumeAction> e)
    {
        switch (e.Action)
        {
            case OsuMusumeAction.Dash:
                dashing = false;
                break;

            case OsuMusumeAction.Down:
                velocity.Y -= 1;
                break;

            case OsuMusumeAction.Up:
                velocity.Y += 1;
                break;

            case OsuMusumeAction.Jump:
                endJump();
                break;
        }
    }
}
