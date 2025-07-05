using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.OsuMusume.UI;

public partial class OsuMusumePlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
{
    protected override Container<Drawable> Content { get; }

    public OsuMusumePlayfieldAdjustmentContainer()
    {
        AddInternal(Content = new DrawSizePreservingFillContainer
        {
            RelativeSizeAxes = Axes.Both,
            TargetDrawSize = new Vector2(256, 256),
            Strategy = DrawSizePreservationStrategy.Minimum,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
        });
    }
}
