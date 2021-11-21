using Godot;

public class CatSpitter : Position2D
{
    private const float Velocity = 1000.0f;
    
    private PackedScene Spit;
    
    public Timer Cooldown => GetNode<Timer>("Cooldown");
    private AudioStreamPlayer Sound => GetNode<AudioStreamPlayer>("SpitSound");

    public override void _Ready()
    {
        this.Spit = (PackedScene)ResourceLoader.Load("res://Scenes/Players/CatSpit.tscn");
    }

    public void Shoot(Vector2 direction)
    {
        if (!Cooldown.IsStopped())
        {
            return;
        }

        CatSpit spit = (CatSpit)this.Spit.Instance();
        spit.GlobalPosition = GlobalPosition;
        spit.LinearVelocity = new Vector2(direction.x * Velocity, direction.y * Velocity);
        spit.SetAsToplevel(true);
        AddChild(spit);
        Sound.Play();
        Cooldown.Start();
    }
}
