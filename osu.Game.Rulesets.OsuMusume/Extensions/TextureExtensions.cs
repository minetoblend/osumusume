using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.OsuMusume.Graphics;

namespace osu.Game.Rulesets.OsuMusume.Extensions;

public static class TextureExtensions
{
    public static DrawableAnimation GetAnimation(this TextureStore store, Character character, CharacterState state, double frameDuration = 80) =>
        store.GetAnimation(character.GetDescription(), state.GetDescription(), frameDuration);

    public static DrawableAnimation GetAnimation(this TextureStore store, string name, [CanBeNull] string type = null, double frameDuration = 80)
    {
        var textures = new List<Texture>();

        int frameCount = 1;

        string suffix = type != null ? $"_{type}" : string.Empty;

        while (store.Get($"{name}{suffix}{frameCount++}") is Texture texture)
            textures.Add(texture);

        var animation = new DrawableAnimation
        {
            DefaultFrameLength = frameDuration,
        };

        foreach (var texture in textures)
            animation.AddFrame(new Sprite { Texture = texture });

        return animation;
    }
}
