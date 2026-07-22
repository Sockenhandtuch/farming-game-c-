using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private AnimatedSprite2D animatedSprite;

    private string direction = "down";

    [Export]
    public float Speed = 150f;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 input = Input.GetVector("left", "right", "up", "down");

        Velocity = input * Speed;
        MoveAndSlide();

        UpdateAnimation(input);
    }

    private void UpdateAnimation(Vector2 input)
    {
        // Spieler bewegt sich
        if (input != Vector2.Zero)
        {
            if (Math.Abs(input.X) > Math.Abs(input.Y))
            {
                direction = "side";
                animatedSprite.FlipH = input.X > 0;
            }
            else
            {
                animatedSprite.FlipH = false;

                if (input.Y < 0)
                    direction = "up";
                else
                    direction = "down";
            }

            animatedSprite.Play("walk_" + direction);
        }
        // Spieler steht
        else
        {
            animatedSprite.Play("idle_" + direction);
        }
    }
}