using Godot;

namespace FarmGame.Tools
{
    [GlobalClass]
    public partial class ToolData : Resource
    {
        [Export]
        public string ToolName { get; set; } = "";

        [Export]
        public ToolType ToolType { get; set; } = ToolType.Hoe;

        [Export]
        public Texture2D Icon { get; set; }

        [Export]
        public int Power { get; set; } = 1;
    }
}