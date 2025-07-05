// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.OsuMusume.UI;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables
{
    public partial class DrawableOsuMusumeHitObject : DrawableHitObject<OsuMusumeHitObject>
    {
        private readonly Container content;

        [Resolved]
        private OsuMusumePlayfield playfield { get; set; }

        public DrawableOsuMusumeHitObject(OsuMusumeHitObject hitObject)
            : base(hitObject)
        {
            Size = new Vector2(24);
            Origin = Anchor.BottomCentre;

            Y = hitObject.Row * OsuMusumePlayfield.ROW_HEIGHT;

            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.Both,
            });
        }

        private Sprite shadow;
        private Drawable shadowProxy;
        private Sprite carrot;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            content.Add(shadow = new Sprite
            {
                Texture = textures.Get("shadow"),
                Scale = new Vector2(0.35f, 0.25f),
                Colour = Color4.DarkBlue,
                Alpha = 0.125f,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.Centre,
            });
            content.Add(new FloatingContainer
            {
                RelativeSizeAxes = Axes.Both,
                StartTime = HitObject.StartTime,
                Children =
                [
                    carrot = new Sprite
                    {
                        Texture = textures.Get("carrot"),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },
                ]
            });

            playfield.ShadowLayer.Add(shadowProxy = shadow.CreateProxy());
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                // todo: implement judgement logic
                ApplyMaxResult();
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            const double duration = 1000;

            switch (state)
            {
                case ArmedState.Hit:
                    carrot.ScaleTo(1.2f, 200, Easing.OutExpo);
                    this.FadeOut(200, Easing.Out).Expire();
                    break;

                case ArmedState.Miss:

                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
