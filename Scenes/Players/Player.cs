using System;

using Godot;

using Object = Godot.Object;

public abstract class Player : KinematicBody2D
{
    protected Vector2 speed = new Vector2(576, 576);
    private readonly int PlayerNumber;
    private Vector2 velocity = Vector2.Zero;

    private Object lastCollision;

    public bool IsHunting { get; set; }
    public int Lives { get; set; }

    protected Player(int playerNumber)
    {
        this.PlayerNumber = playerNumber;
        this.IsHunting = false;
        this.Lives = 3;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 direction = GetDirection();
        this.Rotation = direction.Angle() - (float)(Math.PI / 2);
        this.velocity = direction * this.speed;
        // this.velocity = CalculateMoveVelocity(direction, this.speed);

        KinematicCollision2D collision = MoveAndCollide(this.velocity * delta);
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
        // this.velocity = MoveAndSlide(this.velocity);
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

    private Vector2 GetDirection()
    {
        float x = Input.GetActionStrength($"p{this.PlayerNumber}_right") -
                  Input.GetActionStrength($"p{this.PlayerNumber}_left");
        float y = Input.GetActionStrength($"p{this.PlayerNumber}_down") -
                  Input.GetActionStrength($"p{this.PlayerNumber}_up");

        return new Vector2(x, y);
    }

    private Vector2 CalculateMoveVelocity(Vector2 direction, Vector2 speed)
    {
        Vector2 velocity = this.velocity;
        // velocity.x = speed.x * direction.x;
        // velocity.y = speed.y * direction.y;

        return velocity;
    }
}