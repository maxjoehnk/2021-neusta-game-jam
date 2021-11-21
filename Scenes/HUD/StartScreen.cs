using Godot;

using JetBrains.Annotations;

public class StartScreen : Node2D
{
    private TextureButton StartButton => GetNode<TextureButton>("TextureButton");

    public override void _Ready()
    {
        this.StartButton.GrabFocus();
    }

    [UsedImplicitly]
    private void _on_TextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/Game.tscn");
    }
}