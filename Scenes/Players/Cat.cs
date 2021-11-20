using Godot;
using System;

public class Cat : Player
{
    private CatSpitter Spitter => GetNode<CatSpitter>("AnimatedSprite/CatSpitter");
    private Timer JumpCoolDown => GetNode<Timer>("JumpCooldown");
    private bool usedJump = false;
    private float jumpSpeed = 1000;

    public Cat() : base(1, 0.99f, 0.5f)
    {
        this.speed = new Vector2(1024, 1024);
        this.IsHunting = true;
    }

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

        this.velocity = (this.velocity.Length() + jumpSpeed) * direction;
        JumpCoolDown.Start();
        usedJump = true;
    }

    protected override void PrePhysic()
    {
        if (usedJump)
        {
            usedJump = false;
            float rotation = this.Rotation + (float)(Math.PI / 2);
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            this.velocity = 10 * direction;
        }
    }
}
