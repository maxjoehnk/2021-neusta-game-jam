using System;

using Godot;

using Object = Godot.Object;

public abstract class Player : KinematicBody2D
{
    private readonly int playerNumber;
    
    protected Vector2 Speed = new Vector2(576, 576);
    protected Vector2 Velocity = Vector2.Zero;

    private Object lastCollision;

    public bool IsHunting { get; set; }
    public int Lives { get; private set; }

    public float SpecialCooldown => SpecialTimer?.TimeLeft / SpecialTimer?.WaitTime ?? 0;
    public float AttackCooldown => AttackTimer?.TimeLeft / AttackTimer?.WaitTime ?? 0;

    protected abstract Timer SpecialTimer { get; }
    protected abstract Timer AttackTimer { get; }

    private readonly float lowPassVelocity;
    private readonly float lowPassRotation;

    protected Player(int playerNumber, float lowPassVelocity = 0f, float lowPassRotation = 0f)
    {
        this.playerNumber = playerNumber;
        this.IsHunting = false;
        this.Lives = 3;
        this.lowPassVelocity = lowPassVelocity;
        this.lowPassRotation = lowPassRotation;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = GetDirection();

        SetVelocityAndRotation(delta, direction.Angle() - (float)(Math.PI / 2), direction);
        if (Input.IsActionJustPressed($"p{this.playerNumber}_special"))
        {
            SpecialMove();
        }

        KinematicCollision2D collision = MoveAndCollide(this.Velocity);
        if (collision != null)
        {
            this.Velocity = this.Velocity.Bounce(collision.Normal);

            float rotation = this.Velocity.Angle();
            Vector2 directionVector = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            this.Velocity = (this.Velocity.Length() / 4.0f) * directionVector;
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

        if (Input.IsActionJustPressed($"p{this.playerNumber}_attack"))
        {
            this.Attack();
        }
        PrePhysic();
    }

    public void Reset()
    {
        this.Lives = 3;
        this.IsHunting = false;
    }

    private void SetVelocityAndRotation(float delta, float currentRotation, Vector2 direction)
    {
        this.Velocity = this.Velocity * this.lowPassVelocity + (1.0f - this.lowPassVelocity) * direction * this.Speed * delta;

        if (direction.Length() > 0)
        {
            if ((this.Rotation - currentRotation) > Math.PI)
            {
                this.Rotation -= (float)Math.PI * 2;
            }
            this.Rotation = this.Rotation * this.lowPassRotation + currentRotation * (1.0f - this.lowPassRotation);
        }
    }

    protected abstract void Attack();

    protected abstract void SpecialMove();
    protected abstract void PrePhysic();

    private Vector2 GetDirection()
    {
        float x = Input.GetActionStrength($"p{this.playerNumber}_right") -
                  Input.GetActionStrength($"p{this.playerNumber}_left");
        float y = Input.GetActionStrength($"p{this.playerNumber}_down") -
                  Input.GetActionStrength($"p{this.playerNumber}_up");

        return new Vector2(x, y);
    }
}
