using Godot;

public class PlayerOverlay : Node2D
{
    private AnimatedSprite Heart1 => GetNode<AnimatedSprite>("Heart1");
    private AnimatedSprite Heart2 => GetNode<AnimatedSprite>("Heart2");
    private AnimatedSprite Heart3 => GetNode<AnimatedSprite>("Heart3");

    private AnimatedSprite[] Hearts => new[] { Heart1, Heart2, Heart3 };
    private AnimatedSprite AngryFace => GetNode<AnimatedSprite>("AngryFace");

    private TextureProgress AttackCooldown => GetNode<TextureProgress>("AttackCooldown");
    private TextureProgress SpecialCooldown => GetNode<TextureProgress>("SpecialCooldown");
    
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

    public void SetSpecialCooldown(float cooldown)
    {
        SpecialCooldown.Value = cooldown;
    }

    public void SetAttackCooldown(float cooldown)
    {
        AttackCooldown.Value = cooldown;
    }
}
