using Godot;
using System;

public class CatSpit : RigidBody2D
{
    public void Destroy()
    {
        this.GetParent().RemoveChild(this);
    }
}
