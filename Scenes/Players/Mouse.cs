using Godot;
using System;

public class Mouse : Player
{
    public Mouse() : base(2)
    {
        this.speed = new Vector2(512, 512);
        this.IsHunting = false;
    }
}
