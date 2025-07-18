﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.OsuMusume.Beatmaps;
using osu.Game.Rulesets.OsuMusume.Mods;
using osu.Game.Rulesets.OsuMusume.Replays;
using osu.Game.Rulesets.OsuMusume.UI;
using osu.Game.Rulesets.Replays.Types;

namespace osu.Game.Rulesets.OsuMusume
{
    public class OsuMusumeRuleset : Ruleset
    {
        public override string Description => "osumusume";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableOsuMusumeRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new OsuMusumeBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new OsuMusumeDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.Automation:
                    return [new OsuMusumeModAutoplay()];

                case ModType.DifficultyReduction:
                    return [new OsuMususmeModNofail()];

                default:
                    return [];
            }
        }

        public override string ShortName => "osumusume";

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.W, OsuMusumeAction.Up),
            new KeyBinding(InputKey.A, OsuMusumeAction.Up),
            new KeyBinding(InputKey.D, OsuMusumeAction.Down),
            new KeyBinding(InputKey.S, OsuMusumeAction.Down),
            new KeyBinding(InputKey.Shift, OsuMusumeAction.Dash),
            new KeyBinding(InputKey.Space, OsuMusumeAction.Jump),
            new KeyBinding(InputKey.J, OsuMusumeAction.Hit1),
            new KeyBinding(InputKey.K, OsuMusumeAction.Hit2),
            new KeyBinding(InputKey.L, OsuMusumeAction.Jump),
        };

        public override Drawable CreateIcon() => new OsuMusumeRulesetIcon(this);

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;

        public override IConvertibleReplayFrame CreateConvertibleReplayFrame() => new OsuMusumeReplayFrame();
    }
}
