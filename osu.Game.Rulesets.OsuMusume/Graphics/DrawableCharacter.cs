using System;
using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.OsuMusume.Extensions;
using osu.Game.Rulesets.OsuMusume.Timing;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.Graphics;

public partial class DrawableCharacter : CompositeDrawable
{
    private readonly Character character;

    private readonly Bindable<CharacterState> state = new Bindable<CharacterState>();

    public CharacterState State
    {
        get => state.Value;
        set => state.Value = value;
    }

    private double animationRate = 1;

    public double AnimationRate
    {
        get => animationRate;
        set
        {
            if (animationRate == value)
                return;

            animationRate = value;
            if (LoadState >= LoadState.Loading)
                animationClock.Rate = value;
        }
    }

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    public DrawableCharacter(Character character)
    {
        this.character = character;

        Size = new Vector2(48);
        OriginPosition = new Vector2(24, 39);
        Name = character.ToString();
    }

    private DrawableAnimation runAnimation;

    private RateAdjustableClock animationClock;

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        Clock = animationClock = new RateAdjustableClock(Clock)
        {
            Rate = animationRate,
        };

        InternalChildren =
        [
            runAnimation = textures.GetAnimation(character, CharacterState.Running),
        ];

        foreach (var c in InternalChildren)
            c.Hide();
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        state.BindValueChanged(e =>
        {
            AnimationFor(e.OldValue).Hide();
            AnimationFor(e.NewValue).Show();
        }, true);
    }

    protected DrawableAnimation AnimationFor(CharacterState state) => state switch
    {
        CharacterState.Running => runAnimation,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}

public enum CharacterState
{
    [Description("run")]
    Running,
}

public enum Character
{
    [Description("special_week")]
    SpecialWeek,
}
