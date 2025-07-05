// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.OsuMusume.Scenery;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI
{
    [Cached]
    public partial class OsuMusumePlayfield : ScrollingPlayfield
    {
        private DependencyContainer dependencies = null!;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public Container ShadowLayer { get; private set; }

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

            AddRangeInternal(new Drawable[]
            {
                new RaceTrack(),
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Y = 100,
                    Height = 140,
                    X = 100,
                    Children =
                    [
                        new HitArea(),
                        ShadowLayer = new Container { RelativeSizeAxes = Axes.Both },
                        HitObjectContainer,
                        new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            X = -50,
                            Scale = new Vector2(0.75f),
                            Children =
                            [
                                new DrawableAnimation
                                {
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.Centre,
                                }.With(a =>
                                {
                                    for (int i = 0; i < 8; i++)
                                    {
                                        a.AddFrame(new Sprite
                                        {
                                            Texture = textures.Get($"special_week_{i + 1}"),
                                            X = i % 4 == 2 ? 2 : i % 2 == 1 ? 1 : 0
                                        }, 80);
                                    }
                                })
                            ]
                        }
                    ],
                }
            });
        }
    }
}
