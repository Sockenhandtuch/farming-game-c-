using Godot;
using System;
using FarmGame.Tools;

namespace FarmGame.Player
{
    public partial class Player : CharacterBody2D
    {
        [Export]
        public ToolData[] Tools;

        public ToolData CurrentTool;

        [Export]
        public float Speed = 150f;

        private AnimatedSprite2D animatedSprite;
        private ToolController _toolController;
        private Marker2D interactionPoint;
        private ToolWheel _toolWheel;

        private string direction = "down";


        public override void _Ready()
        {
            animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            _toolController = GetNode<ToolController>("ToolController");
            interactionPoint = GetNode<Marker2D>("InteractionPoint");
            _toolWheel = GetNode<ToolWheel>("%ToolWheel");

            _toolWheel.ToolSelected += OnToolSelected;

            // Erstes Werkzeug aus der Liste als Startwerkzeug setzen
            if (Tools != null && Tools.Length > 0)
            {
                CurrentTool = Tools[0];
                _toolController.CurrentTool = CurrentTool;
            }
        }


        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("use_tool"))
            {
                _toolController.UseTool(interactionPoint.GlobalPosition);
            }

            if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Right && mb.Pressed)
            {
                _toolWheel.Open(Tools);
            }
        }


        private void OnToolSelected(ToolData tool)
        {
            CurrentTool = tool; // kann auch null sein = "kein Werkzeug"
            _toolController.CurrentTool = CurrentTool;

            GD.Print("Werkzeug ausgewählt: " + (CurrentTool != null ? CurrentTool.ToolName : "Keins"));
        }


        public override void _PhysicsProcess(double delta)
        {
            Vector2 input = Input.GetVector("left", "right", "up", "down");

            Velocity = input * Speed;
            MoveAndSlide();

            UpdateAnimation(input);
            UpdateInteractionPoint();
        }

        
        private void UpdateInteractionPoint()
        {
            switch (direction)
            {
                case "up":
                    interactionPoint.Position = new Vector2(0, -16);
                    break;

                case "down":
                    interactionPoint.Position = new Vector2(0, 16);
                    break;

                case "side":
                    if (animatedSprite.FlipH)
                        interactionPoint.Position = new Vector2(16, 0);
                    else
                        interactionPoint.Position = new Vector2(-16, 0);
                    break;
            }
        }


        private void UpdateAnimation(Vector2 input)
        {
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
            else
            {
                animatedSprite.Play("idle_" + direction);
            }
        }
    }
}