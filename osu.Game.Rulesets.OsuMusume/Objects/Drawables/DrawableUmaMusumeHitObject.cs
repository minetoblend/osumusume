// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
            Size = new Vector2(20);
            Origin = Anchor.BottomCentre;

            RelativePositionAxes = Axes.Y;
            Y = (Random.Shared.Next(0, 7) + 0.5f) / 7;

            AddInternal(content = new Container
            {
                RelativeSizeAxes = Axes.Both,
            });
        }

        private Sprite shadow;
        private Drawable shadowProxy;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            // content.Add(shadow = new Sprite
            // {
            //     Texture = textures.Get("shadow"),
            //     Scale = new Vector2(0.5f),
            //     Colour = Color4.DarkBlue,
            //     Alpha = 0.125f,
            //     Anchor = Anchor.BottomCentre,
            //     Origin = Anchor.Centre,
            // });
            // content.Add(new Circle
            // {
            //     RelativeSizeAxes = Axes.Both,
            // });

            // playfield.ShadowLayer.Add(shadow.CreateProxy());
        }

        protected override void Update()
        {
            base.Update();

            content.X = DrawPosition.Y * -0.4f;
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
                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:

                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (shadowProxy != null)
                playfield.ShadowLayer.Remove(shadowProxy, true);
        }
    }
}
