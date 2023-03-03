using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimatedSprite2D animated_sprite;
	public RichTextLabel live_count;
	public override void _Ready()
	{

		animated_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); 
		live_count = GetParent().GetNode<Camera2D>("Camera2D").GetNode<RichTextLabel>("RichTextLabel");
		live_count.Text = "Lives: 3";
	}
	public void kill()
	{
		live_count.Text = "Lives: " + (int.Parse(live_count.Text.Substring(7)) - 1).ToString();
		Position = new Vector2(0, 0);
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		animated_sprite.Play();
		// Add the gravity.
		if (!IsOnFloor())
		{
			animated_sprite.Animation = "jump";
			velocity.Y += gravity * (float)delta;
			if (Position.Y > 50)
				kill();
		}
		else
		{
			if (velocity.X != 0)
			{
				animated_sprite.Animation = "run";
				if (velocity.X > 0)
					animated_sprite.FlipH = false;
				else
					animated_sprite.FlipH = true;
			}
			else
				animated_sprite.Animation = "idle";
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
