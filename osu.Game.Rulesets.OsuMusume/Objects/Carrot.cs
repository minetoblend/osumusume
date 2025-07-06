using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.OsuMusume.Objects;

public class Carrot : OsuMusumeHitObject
{
    private HitObjectProperty<float> row = new HitObjectProperty<float>();

    public Bindable<float> RowBindable => row.Bindable;

    public float Row
    {
        get => row.Value;
        set => row.Value = value;
    }
}
