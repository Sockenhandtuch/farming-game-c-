using Godot;
using System;

public partial class Player : CharacterBody2D
{
	String direction = "side";
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	private void updateSprite(Vector2 inputDirection)
	{
		if (inputDirection != Vector2.Zero)
		{

			if (Math.Abs(inputDirection.X) > Math.Abs(inputDirection.Y))
			{
				direction = "side";
				if (inputDirection.X < 0)
				{
					// animated_sprite.flip_h = false
					animatedSprite.FlipH=false;
				}
				else
				{
					//animated_sprite.flip_h = true
					animatedSprite.FlipH=true;
				}
			}
			else
			{
				//animated_sprite.flip_h = false
				animatedSprite.FlipH=false;

				if (inputDirection.Y < 0)
				{
					direction = "up";
				}
				else
				{
					direction = "down";
				}
			}
			//animatedsprite.play("walk" + direction)
			animatedSprite.Play(direction);
		}
		else
		{
			//animatedsprite.play("idle" + direction)
			animatedSprite.Play("idle");
		}
	}
	private Vector2 GetInputDirection()
	{
		return Input.GetVector("left", "right", "up", "down");
	}

	public const float Speed = 300.0f;

	public override void _PhysicsProcess(double delta)
	{
	Vector2 inputDirection = GetInputDirection();
	updateSprite(inputDirection);

	}

}
