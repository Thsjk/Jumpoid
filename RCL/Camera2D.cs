using Godot;
using System;

public partial class Camera2D : Godot.Camera2D
{
	const int deadZone = 250;
	public Vector2 vector;
	public Vector2 GetVector()
	{
		return vector;
	}
	public void SetVector(Vector2 vector)
	{
		this.vector = vector;
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			Camera2D camera2D = new Camera2D();
			var _target = mouseMotion.Position - GetViewportRect().Size / 2;	
			if (_target.Length() > deadZone)
				SetVector(_target.Normalized());
			else
				SetVector(Vector2.Zero);
		}
	}	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Position += GetVector() * (float)delta * 100;
	}
}
