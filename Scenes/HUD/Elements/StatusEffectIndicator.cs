using System.Drawing.Drawing2D;
using System.Globalization;

using Godot;

public class StatusEffectIndicator : Control
{
    private Label TextLabel => GetNode<Label>("Label");
    private TextureProgress ProgressIndicator => GetNode<TextureProgress>("TextureProgress");

    public string Text { set => TextLabel.Text = value; }
    public float Progress { set => ProgressIndicator.Value = value; }

    public SplitscreenSide Side
    {
        set
        {
            TextLabel.Align = value == SplitscreenSide.Start ? Label.AlignEnum.Left : Label.AlignEnum.Right;
            ProgressIndicator.FillMode = (int)(value == SplitscreenSide.Start ? TextureProgress.FillModeEnum.LeftToRight : TextureProgress.FillModeEnum.RightToLeft);
        }
    }
}
