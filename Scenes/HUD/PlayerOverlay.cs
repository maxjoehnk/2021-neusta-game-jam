using System.Collections.Generic;

using Godot;

public class PlayerOverlay : Container
{
    private SplitscreenSide splitscreenSide;

    public SplitscreenSide Side
    {
        get => this.splitscreenSide;
        set
        {
            this.splitscreenSide = value;
            if (value == SplitscreenSide.Start)
            {
                this.LeftSide.Show();
                this.RightSide.Hide();
            }
            else
            {
                this.RightSide.Show();
                this.LeftSide.Hide();
            }
        }
    }

    private PlayerOverlayInstance LeftSide => GetNode<PlayerOverlayInstance>("LeftAligned");
    private PlayerOverlayInstance RightSide => GetNode<PlayerOverlayInstance>("RightAligned");

    public List<StatusEffectStatus> StatusEffects
    {
        get => this.splitscreenSide == SplitscreenSide.Start
            ? this.LeftSide.StatusEffects
            : this.RightSide.StatusEffects;
        set
        {
            if (this.splitscreenSide == SplitscreenSide.Start)
            {
                this.LeftSide.StatusEffects = value;
            }
            else
            {
                this.RightSide.StatusEffects = value;
            }
        }
    }

    public override void _Ready()
    {
        this.LeftSide.Side = SplitscreenSide.Start;
        this.RightSide.Side = SplitscreenSide.End;
    }

    public void SetHunting(bool hunting)
    {
        this.LeftSide.SetHunting(hunting);
        this.RightSide.SetHunting(hunting);
    }

    public void SetLives(int lives)
    {
        this.LeftSide.SetLives(lives);
        this.RightSide.SetLives(lives);
    }

    public void SetSpecialCooldown(float cooldown)
    {
        this.LeftSide.SetSpecialCooldown(cooldown);
        this.RightSide.SetSpecialCooldown(cooldown);
    }

    public void SetAttackCooldown(float cooldown)
    {
        this.LeftSide.SetAttackCooldown(cooldown);
        this.RightSide.SetAttackCooldown(cooldown);
    }
}