using Godot;

public abstract class StatusEffect : Object
{
    /**
     * Name displayed in HUD
     */
    public abstract string Name { get; }
    
    /**
     * Time this status effect is applied in seconds.
     */
    public abstract float Duration { get; }
    
    public abstract void Apply(IPlayer player);
}