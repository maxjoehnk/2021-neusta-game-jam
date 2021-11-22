using Godot;
using System;

public class Heart : Control
{
    private AnimatedSprite Sprite => GetNode<AnimatedSprite>("Icon");

    [Export]
    public bool Alive
    {
        get => this.Sprite.Animation == "alive";
        set => this.Sprite.Animation = value ? "alive" : "dead";
    }
}
