using Godot;
using System;

public class PlayerOverlay : CanvasLayer
{
    private AnimatedSprite Heart1 => GetNode<AnimatedSprite>("Heart1");
    private AnimatedSprite Heart2 => GetNode<AnimatedSprite>("Heart2");
    private AnimatedSprite Heart3 => GetNode<AnimatedSprite>("Heart3");

    private AnimatedSprite[] Hearts => new[] { Heart1, Heart2, Heart3 };
    private AnimatedSprite AngryFace => GetNode<AnimatedSprite>("AngryFace");

    public void SetHunting(bool hunting)
    {
        if (hunting)
        {
            this.AngryFace.Show();
        }
        else
        {
            this.AngryFace.Hide();
        }
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < this.Hearts.Length; i++)
        {
            this.Hearts[i].Animation = lives > i ? "alive" : "dead";
        }
    }
}
