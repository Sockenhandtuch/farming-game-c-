using Godot;

public partial class World : Node2D
{
    public TileMapLayer Ground { get; private set; }

    public override void _Ready()
    {
        Ground = GetNode<TileMapLayer>("Ground");
    }
}