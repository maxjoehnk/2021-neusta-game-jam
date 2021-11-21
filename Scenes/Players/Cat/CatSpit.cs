using System;

using Godot;

public class CatSpit : RigidBody2D
{
    private AnimatedSprite Sprite => GetNode<AnimatedSprite>("Sprite");
    
    private PackedScene spitOnFloor;
    
    public override void _Ready()
    {
        this.spitOnFloor = (PackedScene)ResourceLoader.Load("res://Scenes/Players/Cat/CatSpitOnFloor.tscn");
    }

    public override void _PhysicsProcess(float delta)
    {
        float speedScale = Math.Min(this.LinearVelocity.Length() / 150, 1);
        this.Sprite.SpeedScale = speedScale;
        if (this.LinearVelocity.Length() < 10)
        {
            this.SpawnOnFloor();
        }
    }

    private void SpawnOnFloor()
    {
        CatSpitOnFloor spit = (CatSpitOnFloor)this.spitOnFloor.Instance();
        spit.Position = this.Position;
        spit.SetAsToplevel(true);
        this.GetParent().AddChild(spit);
        this.GetParent().RemoveChild(this);
        this.QueueFree();
    }
}
