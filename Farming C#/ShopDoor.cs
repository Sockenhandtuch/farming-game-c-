using Godot;

// Wird an ein interagierbares Objekt in der FARM-Szene gehängt
// (z.B. die Tür des eigenen Shops). Läuft der Spieler rein und
// drückt eine Taste, wechseln wir zur Shop-Szene.
public partial class ShopDoor : Area2D
{
	private bool _playerInRange = false;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			_playerInRange = true;
			// Hier könntest du z.B. ein "[E] Shop betreten"-Prompt einblenden
		}
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
			// "shop_entrance" sagt der Shop-Szene, wo der Spieler
			// auftauchen soll (z.B. direkt hinter der Ladentür)
			SceneManager.Instance.GotoScene("shop", "shop_entrance");
		}
	}
}
