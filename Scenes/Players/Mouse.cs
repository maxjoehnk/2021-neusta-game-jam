using System;

using Godot;

public class Mouse : Player
{
    private CheeseThrower Thrower => GetNode<CheeseThrower>("AnimatedSprite/CheeseThrower");
    
    public Mouse() : base(2, 0.0f, 0.0f)
    {
        this.speed = new Vector2(768f, 768f);
        this.IsHunting = false;
    }

    protected override void Attack()
    {
        float rotation = this.Rotation + (float)(Math.PI / 2);
        Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

        Thrower.Throw(direction);
    }

    protected override void useSpecialMove()
    {
    }
    protected override void _Pre_Physic()
    {

    }
}
