using System;

using Godot;

using Object = Godot.Object;

public abstract class Player : KinematicBody2D
{
    protected Vector2 speed = new Vector2(576, 576);
    private readonly int PlayerNumber;
    protected Vector2 velocity = Vector2.Zero;

    private Object lastCollision;

    public bool IsHunting { get; set; }
    public int Lives { get; set; }

    protected float lowpassVel;
    protected float lowpassRot;

    protected Player(int playerNumber, float lowpassVel_in, float lowpassRot_in)
    {
        this.PlayerNumber = playerNumber;
        this.IsHunting = false;
        this.Lives = 3;
        this.lowpassVel = lowpassVel_in;
        this.lowpassRot = lowpassRot_in;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = GetDirection();

        setVelocityAndRotation(delta, direction.Angle() - (float)(Math.PI / 2), direction);

        KinematicCollision2D collision = MoveAndCollide(this.velocity);
        if (collision != null)
        {
            this.velocity = this.velocity.Bounce(collision.Normal);
            if (this.lastCollision != collision.Collider)
            {
                if (collision.Collider.GetType().IsSubclassOf(typeof(Player)))
                {
                    if (!this.IsHunting)
                    {
                        this.Lives -= 1;
                    }
                }
            }

            this.lastCollision = collision.Collider;
        }
        else
        {
            this.lastCollision = null;
        }
        this._PostPhysics();
    }

    public void Reset()
    {
        this.Lives = 3;
        this.IsHunting = false;
    }

    protected virtual void _PostPhysics()
    {
    }
    private void setVelocityAndRotation(float delta, float currentRotation, Vector2 direction)
    {
        this.velocity = this.velocity * lowpassVel + (1.0f - lowpassVel) * direction * this.speed * delta;

        if (direction.Length() > 0)
        {
            float otherRot = currentRotation;
            if ((this.Rotation - currentRotation) > Math.PI)
            {
                this.Rotation -= (float)Math.PI * 2;
            }
            this.Rotation = this.Rotation * lowpassRot + currentRotation * (1.0f - lowpassRot);
        }
    }
    private Vector2 GetDirection()
    {
        float x = Input.GetActionStrength($"p{this.PlayerNumber}_right") -
                  Input.GetActionStrength($"p{this.PlayerNumber}_left");
        float y = Input.GetActionStrength($"p{this.PlayerNumber}_down") -
                  Input.GetActionStrength($"p{this.PlayerNumber}_up");

        return new Vector2(x, y);
    }


}
