using Godot;
using System;

public class Cat : Player
{
    public Cat() : base(1)
    {
        this.speed = new Vector2(576, 576);
        this.IsHunting = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionJustPressed("p1_special"))
        {
            
        }
    }
}
