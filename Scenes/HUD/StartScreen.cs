using Godot;
using System;

public class StartScreen : Node2D
{



private void _on_TextureButton_pressed()
{
	GetTree().ChangeScene("res://Scenes/Game.tscn");
}

}
