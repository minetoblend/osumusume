using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.OsuMusume;

public partial class OsuMusumeRulesetIcon(Ruleset ruleset) : Sprite
{
    private static bool addedStore;

    [BackgroundDependencyLoader(permitNulls: true)]
    private void load(IRenderer renderer, [CanBeNull] OsuGameBase game)
    {
        Texture = new TextureStore(renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/icon");

        if (game != null && !addedStore)
        {
            game.Resources.AddStore(ruleset.CreateResourceStore());
            addedStore = true;
        }
    }
}
