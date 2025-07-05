using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Rulesets.OsuMusume.UI;

namespace osu.Game.Rulesets.OsuMusume.Scenery;

public partial class HorizontalScrollingSprite : CompositeDrawable
{
    public required Texture Texture { get; init; }

    public RectangleF? TextureRect { get; set; }

    private Sprite sprite1, sprite2;

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; } = null;

    [Resolved]
    private IStartTimeProvider startTimeProvider { get; set; } = null!;

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    public float ScrollSpeed { get; set; } = 1;

    private float targetScrollOffset, scrollOffset;

    protected override void Update()
    {
        base.Update();

        float spriteHeight = DrawHeight;
        float spriteWidth = spriteHeight * Texture.Width / Texture.Height;

        int numSprites = (int)(DrawWidth / spriteWidth + 2);

        targetScrollOffset += scrollAlgorithm.GetLength(Time.Current - Time.Elapsed, Time.Current, timeRange, DrawWidth);

        scrollOffset = float.Lerp(targetScrollOffset, scrollOffset, (float)Math.Exp(-0.01f * Time.Elapsed));

        float offset = -scrollOffset % spriteWidth;

        while (offset > 0)
            offset -= spriteWidth;

        for (int i = 0; i < numSprites; i++)
        {
            if (InternalChildren.Count <= i)
                AddInternal(new Sprite { Texture = Texture });

            var sprite = InternalChildren[i];

            sprite.Height = spriteHeight;
            sprite.Width = spriteWidth + 1;
            sprite.X = offset;

            offset += spriteWidth;
        }

        while (InternalChildren.Count > numSprites)
            RemoveInternal(InternalChildren[^1], true);
    }
}
