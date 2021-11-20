using Godot;

public class GameFinishedOverlay : Node2D
{
    private Node2D CatWin => GetNode<Node2D>("CatWin");
    private Node2D MouseWin => GetNode<Node2D>("MouseWin");

    [Signal]
    delegate void RestartGame();
    
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
        if (this.Visible && Input.IsActionJustPressed("restart_game"))
        {
            EmitSignal(nameof(RestartGame));
        }
    }
}