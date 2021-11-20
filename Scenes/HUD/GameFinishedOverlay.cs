using Godot;

public class GameFinishedOverlay : Node2D
{
    private AnimatedSprite CatWin => GetNode<AnimatedSprite>("CatWin");
    private AnimatedSprite MouseWin => GetNode<AnimatedSprite>("MouseWin");

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