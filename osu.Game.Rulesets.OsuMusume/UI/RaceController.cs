using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Utils;
using osu.Game.Rulesets.OsuMusume.Graphics;
using osu.Game.Rulesets.OsuMusume.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class RaceController : CompositeDrawable
{
    [Cached(typeof(IList<IUma>))]
    private readonly List<IUma> characters = new List<IUma>();

    [Cached]
    public readonly PlayerUma Player = new PlayerUma(UmaType.SpecialWeek);

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    [Resolved]
    private IStartTimeProvider startTimeProvider { get; set; }

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    [Resolved]
    private OsuMusumePlayfield playfield { get; set; }

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        RelativeSizeAxes = Axes.Both;

        var characterTypes = Enum.GetValues<UmaType>();

        for (int i = 0; i < 7; i++)
        {
            var character = new EnemyUma(characterTypes[i % characterTypes.Length])
            {
                Row = i,
            };

            characters.Add(character);
            AddInternal(character);
        }

        AddInternal(Player);
        characters.Add(Player);

        for (int i = 0; i < 8; i++)
        {
            AddInternal(new Sprite
            {
                Texture = textures.Get("start_gate"),
                Origin = Anchor.Centre,
                Y = i * OsuMusumePlayfield.ROW_HEIGHT - 20,
            });
        }
    }

    protected override void Update()
    {
        base.Update();

        X = scrollAlgorithm.PositionAt(Math.Max(Time.Current, startTimeProvider.StartTime), Time.Current, timeRange, playfield.DrawWidth);
        float startTimeOffset = scrollAlgorithm.PositionAt(startTimeProvider.StartTime, Time.Current, timeRange, playfield.DrawWidth);

        foreach (var child in InternalChildren.ToList())
        {
            if (child is not IUma)
                child.X = startTimeOffset - X;

            int row = (int)(child.Y / OsuMusumePlayfield.ROW_HEIGHT);

            ChangeInternalChildDepth(child, -row);
        }
    }

    private partial class EnemyUma : CompositeDrawable, IUma
    {
        public int Row { init => targetPosition.Y = Y = (value + 0.5f) * OsuMusumePlayfield.ROW_HEIGHT; }

        private readonly DrawableUma drawableUma;

        public EnemyUma(UmaType umaType)
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.BottomCentre;

            AddInternal(drawableUma = new DrawableUma(umaType)
            {
                Anchor = Anchor.BottomCentre
            });
        }

        [Resolved(CanBeNull = true)]
        private ScoreProcessor scoreProcessor { get; set; }

        [BackgroundDependencyLoader]
        private void load(IStartTimeProvider startTimeProvider)
        {
            Scheduler.AddDelayed(updateVelocity, startTimeProvider.StartTime - Time.Current);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            targetPosition = Position;
        }

        [Resolved]
        private IList<IUma> characters { get; set; }

        [Resolved]
        private IStartTimeProvider startTimeProvider { get; set; }

        [Resolved]
        private IScrollingInfo scrollingInfo { get; set; }

        [Resolved]
        private OsuMusumePlayfield playfield { get; set; }

        [Resolved]
        private PlayerUma player { get; set; }

        private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
        private double timeRange => scrollingInfo.TimeRange.Value;

        private Vector2 targetPosition;

        public Vector2 Velocity { get; private set; }

        private float healthOffset;

        protected override void Update()
        {
            base.Update();

            float rate = scrollAlgorithm.GetLength(Time.Current, Time.Current + 30, timeRange, 100);

            drawableUma.AnimationRate = Time.Current > startTimeProvider.StartTime ? float.Clamp(rate, 0.5f, 1.5f) : 0;

            if (Time.Current < startTimeProvider.StartTime)
                return;

            foreach (var uma in characters)
            {
                if (uma == this)
                    continue;

                float distance = Vector2.Distance(uma.Position, targetPosition);

                if (distance < 15 && distance > 0)
                {
                    var direction = (Position - uma.Position).Normalized();

                    float factor = (float)Time.Elapsed * 0.04f * (Random.Shared.NextSingle() * 0.5f + 0.5f);

                    targetPosition -= direction * (distance - 15) * factor;
                }
            }

            targetPosition += Velocity * (float)Time.Elapsed;
            targetPosition.Y = float.Clamp(targetPosition.Y, 0, 140);

            if (scoreProcessor != null)
            {
                float target = (float)(1 - scoreProcessor.Accuracy.Value) * 100f;

                healthOffset = float.Lerp(target, healthOffset, (float)Math.Exp(-0.001 * Time.Elapsed));
            }

            Position = Vector2.Lerp(targetPosition + new Vector2(healthOffset, 0), Position, (float)Math.Exp(-0.03 * Time.Elapsed));

            foreach (var (_, hitObject) in playfield.HitObjectContainer.AliveEntries)
            {
                if (hitObject is DrawableHurdle)
                {
                    if (hitObject.X - X < 10 && hitObject.X - X > 0 && !isJumping)
                        jump();
                }
            }

            Alpha = Interpolation.ValueAt(Vector2.Distance(Position, player.Position), 0.4f, 1f, 20, 60);
        }

        private bool isJumping => drawableUma.Transforms.Any();

        private void jump()
        {
            drawableUma.FinishTransforms();
            drawableUma.MoveToY(-30, 200, Easing.OutCubic)
                       .Then()
                       .MoveToY(0, 200, Easing.InCubic);
        }

        private void updateVelocity()
        {
            Velocity = new Vector2
            {
                X = (Random.Shared.NextSingle() - 0.5f) * 0.03f,
                Y = (Random.Shared.NextSingle() - 0.5f) * 0.05f,
            };

            if (targetPosition.X < -100)
                X += 0.25f;

            if (targetPosition.X > 0)
                X -= -0.25f;

            if ((targetPosition.Y < 10 && Velocity.Y < 0) || (targetPosition.Y > 130 && Velocity.Y > 0))
                Velocity = Velocity with { Y = -Velocity.Y };

            Scheduler.AddDelayed(updateVelocity, 1000 + Random.Shared.NextSingle() * 3000);
        }
    }
}
