// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.OsuMusume.Objects
{
    public class OsuMusumeHitObject : HitObject
    {
        private HitObjectProperty<int> row = new HitObjectProperty<int>();

        public Bindable<int> RowBindable => row.Bindable;

        public int Row
        {
            get => row.Value;
            set => row.Value = value;
        }

        public override Judgement CreateJudgement() => new Judgement();
    }
}
