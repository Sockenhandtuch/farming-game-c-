using Godot;
using FarmGame.Tools;

namespace FarmGame.Player;

public partial class ToolController : Node
{
    [Export]
    public ToolData CurrentTool;

    public void UseTool()
    {
        if (CurrentTool == null)
            return;

        switch (CurrentTool.ToolType)
        {
            case ToolType.Hoe:
                GD.Print("Hacke benutzt");
                break;

            case ToolType.Axe:
                GD.Print("Axt benutzt");
                break;

            case ToolType.Pickaxe:
                GD.Print("Spitzhacke benutzt");
                break;

            case ToolType.WateringCan:
                GD.Print("Gießkanne benutzt");
                break;

            case ToolType.Scythe:
                GD.Print("Sense benutzt");
                break;
        }
    }
}