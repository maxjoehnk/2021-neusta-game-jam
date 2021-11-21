using Godot;

public class PauseMenu : Control
{
    private TextureButton ResumeButton => GetNode<TextureButton>("CenterContainer2/ResumeButton");
    private TextureButton RestartButton => GetNode<TextureButton>("CenterContainer3/RestartButton");
    private TextureButton ExitButton => GetNode<TextureButton>("CenterContainer4/ExitButton");
    
    [Signal]
    public delegate void ResumeGame();

    [Signal]
    public delegate void RestartGame();

    [Signal]
    public delegate void ExitGame();

    public override void _Ready()
    {
        this.Hide();
        this.ResumeButton.Connect("pressed", this, nameof(this.OnResume));
        this.RestartButton.Connect("pressed", this, nameof(this.OnRestart));
        this.ExitButton.Connect("pressed", this, nameof(this.OnExit));
        this.Connect("visibility_changed", this, nameof(this.OnVisibilityChanged));
    }

    public void OnVisibilityChanged()
    {
        if (this.Visible)
        {
            this.ResumeButton.GrabFocus();
        }
    }

    private void OnResume()
    {
        EmitSignal(nameof(ResumeGame));
        this.Hide();
        GetTree().Paused = false;
    }

    private void OnRestart()
    {
        EmitSignal(nameof(RestartGame));
    }
    
    private void OnExit()
    {
        EmitSignal(nameof(ExitGame));
        GetTree().Quit();
    }
}
