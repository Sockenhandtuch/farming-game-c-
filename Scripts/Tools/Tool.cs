using Godot;
using System;


using Godot;

public partial class Tool : Node
{
    [Export]
    public ToolData Data;

    public virtual void Use()
    {
        GD.Print($"Benutze {Data.ToolName}");
    }
}


public partial class Tool : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
