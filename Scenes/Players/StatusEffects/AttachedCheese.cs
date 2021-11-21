public class AttachedCheese : StatusEffect
{
    public override string Name => "You got cheesed.";

    public override float Duration => 2;

    public override void Apply(IPlayer player)
    {
        player.Speed *= 0.8f;
    }
}