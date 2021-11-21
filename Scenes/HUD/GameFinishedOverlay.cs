using Godot;

public class GameFinishedOverlay : Node2D
{
    private Node2D CatWin => GetNode<Node2D>("CatWin");
    private Node2D MouseWin => GetNode<Node2D>("MouseWin");
    private Timer Timer => GetNode<Timer>("Timer");
    private Label ContinueText => GetNode<Label>("ContinueText");

    [Signal]
    public delegate void RestartGame();

    public override void _Ready()
    {
        this.Connect("visibility_changed", this, nameof(this.OnVisibilityChanged));
        this.Timer.Connect("timeout", this, nameof(this.OnTimeout));
        this.ContinueText.Hide();
    }

    public void SetWinner(PlayerRole winner)
    {
        if (winner == PlayerRole.Cat)
        {
            CatWin.Show();
            MouseWin.Hide();
        }
        else
        {
            MouseWin.Show();
            CatWin.Hide();
        }
    }

    public override void _Process(float delta)
    {
        if (this.Visible && this.Timer.IsStopped() && Input.IsActionJustPressed("restart_game"))
        {
            this.ContinueText.Hide();
            EmitSignal(nameof(RestartGame));
        }
    }

    public void OnVisibilityChanged()
    {
        if (this.Visible)
        {
            this.Timer.Start();
        }
    }

    public void OnTimeout()
    {
        this.ContinueText.Show();
    }
}