using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.OsuMusume.Graphics;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class RaceController : CompositeDrawable
{
    [Cached(typeof(IList<IUma>))]
    private readonly List<IUma> characters = new List<IUma>();

    public readonly PlayerUma Player = new PlayerUma(UmaType.SpecialWeek);

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; }

    [Resolved]
    private IStartTimeProvider startTimeProvider { get; set; }

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    [BackgroundDependencyLoader]
    private void load()
    {
        RelativeSizeAxes = Axes.Both;

        var characterTypes = Enum.GetValues<UmaType>();

        for (int i = 0; i < 7; i++)
        {
            // TODO
            var character = new EnemyUma(characterTypes[i % characterTypes.Length])
            {
                Row = i,
            };

            characters.Add(character);
            AddInternal(character);
        }

        AddInternal(Player);
        characters.Add(Player);
    }

    protected override void Update()
    {
        base.Update();

        X = scrollAlgorithm.PositionAt(Math.Max(Time.Current, startTimeProvider.StartTime), Time.Current, timeRange, 100);

        foreach (var child in InternalChildren.ToList())
        {
            int row = (int)(child.Y / OsuMusumePlayfield.ROW_HEIGHT);

            ChangeInternalChildDepth(child, -row);
        }
    }

    private partial class EnemyUma : CompositeDrawable, IUma
    {
        public int Row { init => targetPosition.Y = Y = (value + 0.5f) * OsuMusumePlayfield.ROW_HEIGHT; }

        public EnemyUma(UmaType umaType)
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.BottomCentre;

            AddInternal(new DrawableUma(umaType)
            {
                Anchor = Anchor.BottomCentre
            });
        }

        [BackgroundDependencyLoader]
        private void load(IStartTimeProvider startTimeProvider)
        {
            Scheduler.AddDelayed(updateVelocity, startTimeProvider.StartTime - Time.Current);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            targetPosition = Position;

            X = (Random.Shared.NextSingle() - 0.5f) * 10f;
        }

        [Resolved]
        private IList<IUma> characters { get; set; }

        private Vector2 targetPosition;

        public Vector2 Velocity { get; private set; }

        protected override void Update()
        {
            base.Update();

            foreach (var uma in characters)
            {
                if (uma == this)
                    continue;

                float distance = Vector2.Distance(uma.Position, targetPosition);

                if (distance < 15 && distance > 0)
                {
                    var direction = (targetPosition - uma.Position).Normalized();

                    float factor = (float)Time.Elapsed * 0.04f * (Random.Shared.NextSingle() * 0.5f + 0.5f);

                    targetPosition -= direction * (distance - 15) * factor;
                }
            }

            targetPosition += Velocity * (float)Time.Elapsed;
            targetPosition.Y = float.Clamp(targetPosition.Y, 0, 140);

            Position = Vector2.Lerp(targetPosition, Position, (float)Math.Exp(-0.03 * Time.Elapsed));
        }

        private void updateVelocity()
        {
            Velocity = new Vector2
            {
                X = (Random.Shared.NextSingle() - 0.5f) * 0.03f,
                Y = (Random.Shared.NextSingle() - 0.5f) * 0.05f,
            };

            Scheduler.AddDelayed(updateVelocity, 1000 + Random.Shared.NextSingle() * 3000);
        }
    }
}
