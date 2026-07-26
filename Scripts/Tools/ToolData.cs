using Godot;
using FarmGame.Tools;

[GlobalClass]
public partial class ToolData : Resource
{
    [Export]
    public string ToolName = "";

    [Export]
    public ToolType ToolType = ToolType.None;

    [Export]
    public Texture2D Icon;

    [Export]
    public int Power = 1;

    [Export]
    public float UseCooldown = 0.3f;
}
