using Godot;

using JetBrains.Annotations;

public class Cheese : RigidBody2D
{
    public override void _PhysicsProcess(float delta)
    {
        foreach (object collidingBody in this.GetCollidingBodies())
        {
            if (collidingBody is Cat cat)
            {
                cat.AttachStatusEffect(new AttachedCheese());
                this.Destroy();
            }
        }
    }

    [UsedImplicitly]
    public void Destroy()
    {
        this.GetParent().RemoveChild(this);
        QueueFree();
    }
}
