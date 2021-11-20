using Godot;
using System;

public class Cat : Player
{
    private CatSpitter Spitter => GetNode<CatSpitter>("AnimatedSprite/CatSpitter");
    
    public Cat() : base(1)
    {
        this.speed = new Vector2(576, 576);
        this.IsHunting = true;
    }

    protected override void _PostPhysics()
    {
        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        
        if (Input.IsActionJustPressed("p1_attack"))
        {
            Spitter.Shoot(direction);
        }
    }
}
