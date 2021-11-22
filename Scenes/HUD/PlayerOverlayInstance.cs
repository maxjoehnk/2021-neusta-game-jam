using System.Collections.Generic;

using Godot;

public class PlayerOverlayInstance : Control
{
    private PackedScene effectIndicator;

    private Heart Heart1 => GetNode<Heart>("PlayerStatus/Heart1");
    private Heart Heart2 => GetNode<Heart>("PlayerStatus/Heart2");
    private Heart Heart3 => GetNode<Heart>("PlayerStatus/Heart3");

    private Heart[] Hearts => new[] { Heart1, Heart2, Heart3 };
    private Control AngryFace => GetNode<Control>("PlayerStatus/AngryFace");

    private TextureProgress AttackCooldown => GetNode<TextureProgress>("AttackCooldown");
    private TextureProgress SpecialCooldown => GetNode<TextureProgress>("SpecialCooldown");

    private VBoxContainer StatusEffectList => GetNode<VBoxContainer>("StatusEffects");

    public List<StatusEffectStatus> StatusEffects = new List<StatusEffectStatus>()
    {
        new StatusEffectStatus { Duration = 1, Name = "Test" }
    };

    public SplitscreenSide Side { get; set; }

    public override void _Ready()
    {
        this.effectIndicator = (PackedScene)ResourceLoader.Load("res://Scenes/HUD/Elements/StatusEffectIndicator.tscn");
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
            indicator.Side = this.Side;
            indicator.Progress = effectStatus.Duration;
            indicator.Text = effectStatus.Name;
        }
    }

    public void SetHunting(bool hunting)
    {
        this.AngryFace.Visible = hunting;
    }

    public void SetLives(int lives)
    {
        for (int i = 0; i < this.Hearts.Length; i++)
        {
            this.Hearts[i].Alive = lives > i;
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