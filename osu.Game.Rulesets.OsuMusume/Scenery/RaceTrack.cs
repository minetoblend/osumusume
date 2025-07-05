using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.Scenery;

public partial class RaceTrack : CompositeDrawable
{
    [BackgroundDependencyLoader]
    private void load(TextureStore textures)
    {
        RelativeSizeAxes = Axes.X;

        Y = 100;
        Height = 140;

        for (int i = 0; i < 7; i++)
        {
            AddInternal(new HorizontalScrollingSprite
            {
                Texture = textures.Get($"grass_lane_{((i + 1) % 4) + 1}"),
                RelativeSizeAxes = Axes.X,
                Height = 24,
                Y = i * 20,
                ScrollSpeed = 0.9f + (i / 6f) * 0.15f
            });
        }

        AddRangeInternal([
            new HorizontalScrollingSprite
            {
                Texture = textures.Get("dirt"),
                RelativeSizeAxes = Axes.X,
                Height = 20,
                Y = 140,
                ScrollSpeed = 1.05f,
                Depth = 1,
            },
            new HorizontalScrollingSprite
            {
                Texture = textures.Get("dirt"),
                RelativeSizeAxes = Axes.X,
                Height = 20,
                ScrollSpeed = 0.9f,
                Depth = 1,
                Y = 2,
                Origin = Anchor.BottomLeft,
                Scale = new Vector2(1, 0.5f)
            },
            new HorizontalScrollingSprite
            {
                Texture = textures.Get("barrier_top"),
                RelativeSizeAxes = Axes.X,
                Height = 12,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomLeft,
                ScrollSpeed = 0.9f,
                Y = 1,
            },
            new HorizontalScrollingSprite
            {
                Texture = textures.Get("barrier_bottom"),
                RelativeSizeAxes = Axes.X,
                Height = 12,
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Y = 1,
                ScrollSpeed = 1.05f,
            },
        ]);
    }
}
