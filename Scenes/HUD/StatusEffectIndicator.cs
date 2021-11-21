using Godot;

public class StatusEffectIndicator : Control
{
    private Label TextLabel => GetNode<Label>("Label");
    private TextureProgress ProgressIndicator => GetNode<TextureProgress>("TextureProgress");

    public string Text { set => TextLabel.Text = value; }
    public float Progress { set => ProgressIndicator.Value = value; }
}
