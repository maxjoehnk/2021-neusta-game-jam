using System.Collections.Generic;

using Godot;

public class PlayerOverlay : Node2D
{
    private PackedScene effectIndicator;
    
    private AnimatedSprite Heart1 => GetNode<AnimatedSprite>("Heart1");
    private AnimatedSprite Heart2 => GetNode<AnimatedSprite>("Heart2");
    private AnimatedSprite Heart3 => GetNode<AnimatedSprite>("Heart3");

    private AnimatedSprite[] Hearts => new[] { Heart1, Heart2, Heart3 };
    private AnimatedSprite AngryFace => GetNode<AnimatedSprite>("AngryFace");

    private TextureProgress AttackCooldown => GetNode<TextureProgress>("AttackCooldown");
    private TextureProgress SpecialCooldown => GetNode<TextureProgress>("SpecialCooldown");

    private VBoxContainer StatusEffectList => GetNode<VBoxContainer>("StatusEffects");

    public List<StatusEffectStatus> StatusEffects = new List<StatusEffectStatus>();

    public override void _Ready()
    {
        this.effectIndicator = (PackedScene)ResourceLoader.Load("res://Scenes/HUD/StatusEffectIndicator.tscn");
    }

    public override void _Process(float delta)
    {
        foreach (Node child in StatusEffectList.GetChildren())
        {
            StatusEffectList.RemoveChild(child);
        }

        foreach (StatusEffectStatus effectStatus in this.StatusEffects)
        {
            StatusEffectIndicator indicator = this.effectIndicator.Instance<StatusEffectIndicator>();
            StatusEffectList.AddChild(indicator);
            indicator.Progress = effectStatus.Duration;
            indicator.Text = effectStatus.Name;
        }
    }

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
