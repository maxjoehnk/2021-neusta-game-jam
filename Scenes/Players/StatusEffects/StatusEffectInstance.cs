using Godot;

public class StatusEffectInstance : Node
{
    [Signal]
    public delegate void OnTimeout();
    
    public Timer Duration => GetNode<Timer>("Duration");

    public override void _Ready()
    {
        this.Duration.Connect("timeout", this, nameof(this.Timeout));
    }

    public void Setup(StatusEffect effect)
    {
        this.Duration.WaitTime = effect.Duration;
    }

    public void Timeout()
    {
        EmitSignal(nameof(OnTimeout));
        QueueFree();
    }
}
