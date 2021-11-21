using Godot;

public class CatSpitOnFloor : Area2D
{
    private AnimatedSprite Sprite => GetNode<AnimatedSprite>("Sprite");
    private AnimationPlayer AnimationPlayer => GetNode<AnimationPlayer>("AnimationPlayer");
    private Timer Timer => GetNode<Timer>("Timer");

    public override void _Ready()
    {
        this.Sprite.Frame = 0;
        this.Sprite.Play("spawn");
        this.Timer.Connect("timeout", this, nameof(this.Destroy));
        this.Connect("body_entered", this, nameof(this.OnCollision));
        this.AnimationPlayer.Connect("animation_finished", this, nameof(this.Despawn));
    }
    
    public void Destroy()
    {
        this.AnimationPlayer.Play("despawn");
    }

    public void OnCollision(Node body)
    {
        if (body is Mouse mouse)
        {
            mouse.AttachStatusEffect(new AttachedSpit());
        }
    }

    public void Despawn(string animationName)
    {
        this.GetParent().RemoveChild(this);
        this.QueueFree();
    }
}
