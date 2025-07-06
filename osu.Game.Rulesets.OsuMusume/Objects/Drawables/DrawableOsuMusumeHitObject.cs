// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.OsuMusume.Objects.Drawables
{
    public partial class DrawableOsuMusumeHitObject : DrawableHitObject<OsuMusumeHitObject>
    {
        public DrawableOsuMusumeHitObject(OsuMusumeHitObject hitObject)
            : base(hitObject)
        {
        }
    }

    public partial class DrawableOsuMusumeHitObject<T> : DrawableOsuMusumeHitObject
        where T : OsuMusumeHitObject
    {
        public DrawableOsuMusumeHitObject(T hitObject)
            : base(hitObject)
        {
        }

        public new T HitObject => (T)base.HitObject;
    }
}
