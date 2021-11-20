using Godot;

public class Mouse : Player
{
    public Mouse() : base(2, 0.0f, 0.0f)
    {
        this.speed = new Vector2(768f, 768f);
        this.IsHunting = false;
    }
}
