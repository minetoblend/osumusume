// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using JetBrains.Annotations;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play.HUD;

namespace osu.Game.Rulesets.OsuMusume
{
    public partial class OsuMusumeInputManager : RulesetInputManager<OsuMusumeAction>
    {
        public OsuMusumeInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }

        protected override KeyBindingContainer<OsuMusumeAction> CreateKeyBindingContainer(RulesetInfo ruleset, int variant, SimultaneousBindingMode unique) =>
            new OsuMusumeKeyBindingContainer(ruleset, variant, unique);

        private partial class OsuMusumeKeyBindingContainer : RulesetKeyBindingContainer
        {
            public OsuMusumeKeyBindingContainer([NotNull] RulesetInfo ruleset, int variant, SimultaneousBindingMode unique)
                : base(ruleset, variant, unique)
            {
            }

            public override void Add(Drawable drawable)
            {
                if (drawable is KeyCounterActionTrigger<OsuMusumeAction> trigger)
                    trigger.Name = trigger.Action.GetDescription();

                base.Add(drawable);
            }
        }
    }

    public enum OsuMusumeAction
    {
        [Description("Up")]
        Up,

        [Description("Down")]
        Down,

        [Description("Dash")]
        Dash,

        [Description("Jump")]
        Jump,

        [Description("Hit 1")]
        Hit1,

        [Description("Hit 2")]
        Hit2,
    }
}
