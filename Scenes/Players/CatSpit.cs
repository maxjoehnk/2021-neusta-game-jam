using Godot;

using JetBrains.Annotations;

public class CatSpit : RigidBody2D
{
    [UsedImplicitly]
    public void Destroy()
    {
        this.GetParent().RemoveChild(this);
    }
}
