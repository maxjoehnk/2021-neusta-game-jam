using System;

using Godot;

public class Cat : Player
{
    private CatSpitter Spitter => GetNode<CatSpitter>("AnimatedSprite/CatSpitter");
    private Timer JumpCoolDown => GetNode<Timer>("JumpCooldown");
    private Timer DashTime => GetNode<Timer>("JumpCooldown/DashTime");
    
    private bool usedJump = false;
    private float jumpSpeed = 50;

    public Cat() : base(1, 2048f, 0.99f, 0.5f)
    {
        this.IsHunting = true;
    }

    protected override Timer SpecialTimer => JumpCoolDown;
    protected override Timer AttackTimer => Spitter.Cooldown;

    protected override void Attack()
    {
        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        Spitter.Shoot(direction);
    }

    protected override void SpecialMove()
    {
        if (!JumpCoolDown.IsStopped())
        {
            return;
        }

        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        this.Velocity = (this.Velocity.Length() + jumpSpeed) * direction;
        JumpCoolDown.Start();
        DashTime.Start();
        usedJump = true;
        this.PlaySpecialAnimation();
    }

    protected override void PrePhysic()
    {
        if (!this.usedJump || !DashTime.IsStopped())
        {
            return;
        }

        this.usedJump = false;
        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        this.Velocity = 10 * direction;
    }
}
