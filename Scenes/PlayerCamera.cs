using Godot;
using System;

public class PlayerCamera : Camera2D
{
    public Node2D Target { get; set; }

    public override void _PhysicsProcess(float delta)
    {
        this.Position = Target.Position;
    }
}
