using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.OsuMusume.Objects;

public class Carrot : OsuMusumeHitObject
{
    private HitObjectProperty<int> row = new HitObjectProperty<int>();

    public Bindable<int> RowBindable => row.Bindable;

    public int Row
    {
        get => row.Value;
        set => row.Value = value;
    }
}
