using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;

public abstract class Player : KinematicBody2D, IPlayer
{
    private PackedScene statusEffectScene;
            
    private readonly int playerNumber;
    private readonly Godot.Collections.Dictionary<StatusEffect, StatusEffectInstance> statusEffects = new Godot.Collections.Dictionary<StatusEffect, StatusEffectInstance>();

    private readonly float speed;
    private float currentSpeed;
    protected Vector2 Velocity = Vector2.Zero;

    private Object lastCollision;
    
    private Sprite AngryEyes => GetNode<Sprite>("AngryEyes");
    protected AnimatedSprite Sprite => GetNode<AnimatedSprite>("AnimatedSprite");

    public bool IsHunting { get; set; }
    public int Lives { get; set; }

    public float SpecialCooldown => SpecialTimer?.TimeLeft / SpecialTimer?.WaitTime ?? 0;
    public float AttackCooldown => AttackTimer?.TimeLeft / AttackTimer?.WaitTime ?? 0;

    protected abstract Timer SpecialTimer { get; }
    protected abstract Timer AttackTimer { get; }

    private readonly float lowPassVelocity;
    private readonly float lowPassRotation;
    private bool specialAnimationRunning;

    [Signal]
    public delegate void PlayerHit();
    
    protected Player(int playerNumber, float speed, float lowPassVelocity = 0f, float lowPassRotation = 0f)
    {
        this.playerNumber = playerNumber;
        this.speed = speed;
        this.currentSpeed = speed;
        this.IsHunting = false;
        this.Lives = 3;
        this.lowPassVelocity = lowPassVelocity;
        this.lowPassRotation = lowPassRotation;
    }

    public override void _Ready()
    {
        this.statusEffectScene = (PackedScene)ResourceLoader.Load("res://Scenes/Players/StatusEffects/StatusEffectInstance.tscn");
    }

    public override void _Process(float delta)
    {
        this.AngryEyes.Visible = this.IsHunting;
        if (!this.specialAnimationRunning)
        {
            this.Sprite.Animation = this.Velocity.Length() < 0.1 ? "standing" : "default";
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        ApplyStatusEffects();
        Vector2 direction = GetDirection();

        SetVelocityAndRotation(delta, direction.Angle() - (Mathf.Pi / 2), direction);
        if (Input.IsActionJustPressed($"p{this.playerNumber}_special"))
        {
            SpecialMove();
        }

        KinematicCollision2D collision = MoveAndCollide(this.Velocity);
        if (collision != null)
        {
            this.Velocity = this.Velocity.Bounce(collision.Normal);

            float rotation = this.Velocity.Angle();
            Vector2 directionVector = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
            this.Velocity = (this.Velocity.Length() / 4.0f) * directionVector;
            if (this.lastCollision != collision.Collider)
            {
                if (collision.Collider.GetType().IsSubclassOf(typeof(Player)))
                {
                    if (!this.IsHunting)
                    {
                        this.Lives -= 1;
                    }
                    else
                    {
                        ((Player)collision.Collider).Lives -= 1;
                    }
                    EmitSignal(nameof(PlayerHit));
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

    private void ApplyStatusEffects()
    {
        this.currentSpeed = this.speed;
        foreach (StatusEffect statusEffect in this.statusEffects.Keys)
        {
            statusEffect.Apply(this);
        }
    }

    public void Reset()
    {
        this.statusEffects.Clear();
        this.Velocity = Vector2.Zero;
        this.ClearCooldowns();
    }

    public void AttachStatusEffect(StatusEffect effect)
    {
        GD.Print($"{this.GetType().Name} got effect: '{effect.Name}'");
        StatusEffectInstance effectInstance = CreateStatusEffectInstance(effect);
        this.AddChild(effectInstance);
        this.statusEffects.Add(effect, effectInstance);
    }

    private StatusEffectInstance CreateStatusEffectInstance(StatusEffect effect)
    {
        StatusEffectInstance effectInstance = (StatusEffectInstance)this.statusEffectScene.Instance();
        effectInstance.Setup(effect);
        effectInstance.Connect(nameof(StatusEffectInstance.OnTimeout), this, nameof(this.OnStatusEffectTimeout), new Array(effectInstance, effect));

        return effectInstance;
    }

    private void SetVelocityAndRotation(float delta, float currentRotation, Vector2 direction)
    {
        this.Velocity = this.Velocity * this.lowPassVelocity + (1.0f - this.lowPassVelocity) * direction * this.currentSpeed * delta;

        if (direction.Length() > 0)
        {
            if ((this.Rotation - currentRotation) > Mathf.Pi)
            {
                this.Rotation -= Mathf.Pi * 2;
            }
            this.Rotation = this.Rotation * this.lowPassRotation + currentRotation * (1.0f - this.lowPassRotation);
        }
    }

    protected abstract void Attack();

    protected abstract void SpecialMove();
    protected abstract void PrePhysic();

    private void ClearCooldowns()
    {
        AttackTimer?.Stop();
        SpecialTimer?.Stop();
    }

    private Vector2 GetDirection()
    {
        float x = Input.GetActionStrength($"p{this.playerNumber}_right") -
                  Input.GetActionStrength($"p{this.playerNumber}_left");
        float y = Input.GetActionStrength($"p{this.playerNumber}_down") -
                  Input.GetActionStrength($"p{this.playerNumber}_up");

        return new Vector2(x, y);
    }

    public float Speed
    {
        get => this.currentSpeed;
        set => this.currentSpeed = value;
    }

    public void OnStatusEffectTimeout(StatusEffectInstance effectInstance, StatusEffect effect)
    {
        GD.Print($"{this.GetType().Name} lost effect: '{effect.Name}'");
        this.statusEffects.Remove(effect);
        this.RemoveChild(effectInstance);
    }

    public List<StatusEffectStatus> GetStatusEffects()
    {
        return this.statusEffects
            .Select(e => new StatusEffectStatus
            {
                Name = e.Key.Name,
                Duration = e.Value.Duration.TimeLeft / e.Value.Duration.WaitTime
            })
            .ToList();
    }

    // TODO[max]: this is not how inheritance should be used.
    protected void PlaySpecialAnimation()
    {
        this.specialAnimationRunning = true;
        Sprite.Animation = "special";
        Sprite.Connect("animation_finished", this, nameof(this.OnSpecialAnimationDone));
    }

    public void OnSpecialAnimationDone()
    {
        this.Sprite.Disconnect("animation_finished", this, nameof(this.OnSpecialAnimationDone));
        this.specialAnimationRunning = false;
    }
}
