// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.OsuMusume.Graphics;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI
{
    [Cached]
    public partial class OsuMusumePlayfield : ScrollingPlayfield
    {
        public const float ROW_HEIGHT = 20;

        private DependencyContainer dependencies = null!;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public Container ShadowLayer { get; private set; }

        public Container BackgroundLayer { get; private set; }

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer, GameHost host, TextureStore parentStore)
        {
            var resources = new OsuMusumeRuleset().CreateResourceStore();

            TextureStore textures = new TextureStore(
                renderer: renderer,
                store: host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(resources, "Textures")),
                filteringMode: TextureFilteringMode.Nearest,
                scaleAdjust: 1
            );

            dependencies.CacheAs(textures);

            Container content;
            Container offsetContent;

            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.X,
                Y = 100,
                Height = 140,
            });

            content.AddRange([
                new EndlessScrollingSprite
                {
                    Texture = textures.Get("dirt"),
                    RelativeSizeAxes = Axes.X,
                    Height = 20,
                    Y = 140,
                    ScrollSpeed = 1.05f,
                    Depth = 1,
                },
                new EndlessScrollingSprite
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
                new EndlessScrollingSprite
                {
                    Texture = textures.Get("barrier_top"),
                    RelativeSizeAxes = Axes.X,
                    Height = 12,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.BottomLeft,
                    ScrollSpeed = 0.9f,
                    Y = 1,
                },
            ]);

            for (int i = 0; i < 7; i++)
            {
                content.Add(new EndlessScrollingSprite
                {
                    Texture = textures.Get($"grass_lane_{((i + 1) % 4) + 1}"),
                    RelativeSizeAxes = Axes.X,
                    Height = 24,
                    Y = i * 20,
                    ScrollSpeed = 0.9f + (i / 6f) * 0.15f
                });
            }

            var raceController = new RaceController();

            dependencies.CacheAs(raceController.Player);

            content.Add(new Container
            {
                RelativeSizeAxes = Axes.Both,
                X = 100,
                Children =
                [
                    ShadowLayer = new Container { RelativeSizeAxes = Axes.Both },
                    BackgroundLayer = new Container { RelativeSizeAxes = Axes.Both },
                    HitObjectContainer,
                    raceController,
                ]
            });

            content.Add(new EndlessScrollingSprite
            {
                Texture = textures.Get("barrier_bottom"),
                RelativeSizeAxes = Axes.X,
                Height = 12,
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.BottomLeft,
                Y = 1,
                ScrollSpeed = 1.05f,
            });
        }
    }
}
