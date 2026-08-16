using Godot;

// Analog zu ShopDoor.cs, aber für den Dungeon-Eingang. Zeigt, dass
// das gleiche Muster für beliebig viele Übergänge wiederverwendbar ist.
public partial class DungeonEntrance : Area2D
{
	[Export] public string DungeonId { get; set; } = "dungeon_1";

	private bool _playerInRange = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
			_playerInRange = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (body.IsInGroup("player"))
			_playerInRange = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (_playerInRange && @event.IsActionPressed("interact"))
		{
			// GameState merkt sich, welcher Dungeon geladen werden soll -
			// so kann die Dungeon-Szene beim Start prüfen, welches
			// Layout/Material sie generieren bzw. anzeigen muss.
			GameState.Instance.CurrentDungeonId = DungeonId;
			SceneManager.Instance.GotoScene("dungeon", "dungeon_start");
		}
	}
}
