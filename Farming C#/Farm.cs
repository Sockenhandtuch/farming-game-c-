using Godot;
using System.Collections.Generic;

public partial class World : Node2D
{
    public TileMapLayer Ground { get; private set; }

    public override void _Ready()
    {
        Ground = GetNode<TileMapLayer>("Ground");
    }
}

// Wurzel-Skript der FARM-Szene. Findet beim Start heraus, WO der
// Spieler stehen soll (z.B. am Dungeon-Ausgang statt am Kartenrand,
// wenn er aus dem Dungeon zurückkommt).
public partial class Farm : Node2D
{
	private CharacterBody2D _player;
	private Dictionary<string, Node2D> _spawnPoints;

	public override void _Ready()
	{
		_player = GetNode<CharacterBody2D>("Player");

		// Alle möglichen Spawn-Punkte, benannt über Marker2D-Nodes
		// im Szenenbaum. Beliebig viele im Editor platzierbar,
		// ohne Code anzufassen.
		_spawnPoints = new Dictionary<string, Node2D>
		{
			{ "default", GetNode<Node2D>("SpawnPoints/Default") },
			{ "shop_exit", GetNode<Node2D>("SpawnPoints/ShopExit") },
			{ "dungeon_exit", GetNode<Node2D>("SpawnPoints/DungeonExit") },
		};

		string spawnKey = SceneManager.Instance.GetSpawnPoint();
		Node2D targetPoint = _spawnPoints.TryGetValue(spawnKey, out var point)
			? point
			: _spawnPoints["default"];

		_player.GlobalPosition = targetPoint.GlobalPosition;

		// UI z.B. für Gold-Anzeige an GameState-Signal hängen,
		// statt Gold-Wert manuell zu kopieren
		GameState.Instance.GoldChanged += OnGoldChanged;
		OnGoldChanged(GameState.Instance.Gold);
	}

	private void OnGoldChanged(int newAmount)
	{
		// Hier würdest du z.B. ein Label in der UI aktualisieren
	}
}
