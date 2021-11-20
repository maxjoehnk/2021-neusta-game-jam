using System;

using Godot;

public class Mouse : Player
{
    private CheeseThrower Thrower => GetNode<CheeseThrower>("AnimatedSprite/CheeseThrower");
    private Sprite AngryEyes => GetNode<Sprite>("AngryEyes");

    protected override Timer SpecialTimer { get; }
    protected override Timer AttackTimer => Thrower.Cooldown;
    
    public Mouse() : base(2)
    {
        this.Speed = new Vector2(768f, 768f);
        this.IsHunting = false;
    }

    public override void _Process(float delta)
    {
        if (this.IsHunting)
        {
            this.AngryEyes.Show();
        }
        else
        {
            this.AngryEyes.Hide();
        }
    }

    protected override void Attack()
    {
        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        Thrower.Throw(direction);
    }

    protected override void SpecialMove()
    {
    }
    protected override void PrePhysic()
    {

    }
}
