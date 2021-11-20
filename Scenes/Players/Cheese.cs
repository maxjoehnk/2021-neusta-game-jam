using Godot;
using System;

public class Cheese : RigidBody2D
{
    public void Destroy()
    {
        this.GetParent().RemoveChild(this);
    }
}
