using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.OsuMusume.Graphics;
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

    public PlayerUma(UmaType type)
    {
        RelativeSizeAxes = Axes.Both;

        AddInternal(drawableUma = new DrawableUma(type));
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

    public bool OnPressed(KeyBindingPressEvent<OsuMusumeAction> e)
    {
        if (e.Action == OsuMusumeAction.Dash)
            dashing = true;

        velocity += e.Action switch
        {
            OsuMusumeAction.Up => new Vector2(0, -1),
            OsuMusumeAction.Down => new Vector2(0, 1),
            _ => new Vector2(),
        };

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<OsuMusumeAction> e)
    {
        if (e.Action == OsuMusumeAction.Dash)
            dashing = false;

        velocity -= e.Action switch
        {
            OsuMusumeAction.Up => new Vector2(0, -1),
            OsuMusumeAction.Down => new Vector2(0, 1),
            _ => new Vector2(),
        };
    }
}
