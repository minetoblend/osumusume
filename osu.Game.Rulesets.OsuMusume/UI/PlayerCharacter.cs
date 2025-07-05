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

public partial class PlayerCharacter : CompositeDrawable, IKeyBindingHandler<OsuMusumeAction>
{
    private Vector2 position;
    private Vector2 velocity;

    private DrawableCharacter character;

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    [Resolved]
    private IStartTimeProvider startTimeProvider { get; set; }

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    public PlayerCharacter(Character character)
    {
        RelativeSizeAxes = Axes.Both;

        AddInternal(this.character = new DrawableCharacter(Character.SpecialWeek));
    }

    protected override void Update()
    {
        base.Update();

        float rate = Time.Current > startTimeProvider.StartTime ? scrollAlgorithm.GetLength(Time.Current, Time.Current + 30, timeRange, 100) : 0;

        character.AnimationRate = float.Clamp(rate, 0.5f, 1.5f);

        position = Vector2.Clamp(position + velocity * (float)Time.Elapsed * 0.2f, Vector2.Zero, new Vector2(0, 130));

        character.Position = Vector2.Lerp(position, character.Position, (float)Math.Exp(-0.02f * Time.Elapsed));
    }

    public bool OnPressed(KeyBindingPressEvent<OsuMusumeAction> e)
    {
        velocity.Y += e.Action switch
        {
            OsuMusumeAction.Up => -1,
            OsuMusumeAction.Down => 1,
            _ => throw new ArgumentOutOfRangeException()
        };

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<OsuMusumeAction> e)
    {
        velocity.Y -= e.Action switch
        {
            OsuMusumeAction.Up => -1,
            OsuMusumeAction.Down => 1,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
