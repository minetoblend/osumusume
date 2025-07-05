using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;

namespace osu.Game.Rulesets.OsuMusume.Scenery;

public partial class Grass : CompositeDrawable
{
    private Texture grassTexture = null!;

    private IScrollAlgorithm scrollAlgorithm => scrollingInfo.Algorithm.Value;
    private double timeRange => scrollingInfo.TimeRange.Value;

    [Resolved]
    private IScrollingInfo scrollingInfo { get; set; } = null;

    private Sprite sprite;

    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        grassTexture = textures.Get("grass_striped", wrapModeS: WrapMode.Repeat, wrapModeT: default);

        RelativeSizeAxes = Axes.Both;

        AddInternal(sprite = new Sprite
        {
            Texture = grassTexture,
            TextureRelativeSizeAxes = Axes.None,
        });
    }

    protected override void Update()
    {
        base.Update();

        float spriteHeight = DrawHeight;
        float spriteWidth = spriteHeight * grassTexture.Width / grassTexture.Height;

        sprite.Height = DrawHeight;
        sprite.Width = DrawWidth * 2;

        float offset = scrollAlgorithm.PositionAt(0, Time.Current, timeRange, DrawWidth);

        sprite.TextureRectangle = new RectangleF(0, 0, spriteWidth, spriteHeight);

        sprite.X = offset % spriteWidth;
    }
}
