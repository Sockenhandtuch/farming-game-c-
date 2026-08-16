using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

// Autoload-Singleton: "SceneManager"
//
// Zentrale Stelle für alle Szenenwechsel im Spiel.
// Andere Skripte rufen einfach SceneManager.Instance.GotoScene("shop") auf.
//
// Wichtig für C#-Autoloads in Godot: Man registriert sie in
// Projekteinstellungen > Autoload genauso wie GDScript-Dateien -
// einfach den Pfad zur .cs-Datei auswählen, Node Name z.B. "SceneManager"
// eintragen. Godot erstellt daraus automatisch eine Node-Instanz.
public partial class SceneManager : Node
{
	// Statischer Zugriff, damit man nicht in jeder Szene erst
	// GetNode<SceneManager>("/root/SceneManager") schreiben muss.
	public static SceneManager Instance { get; private set; }

	private readonly Dictionary<string, string> _scenes = new()
	{
		{ "farm", "res://scenes/farm.tscn" },
		{ "shop", "res://scenes/shop.tscn" },
		{ "dungeon", "res://scenes/dungeon.tscn" },
	};

	private string _nextSpawnPoint = "";

	private CanvasLayer _fadeLayer;
	private ColorRect _fadeRect;

	public override void _Ready()
	{
		Instance = this;

		// Erwartet im Szenenbaum des SceneManager-Autoloads:
		// SceneManager (dieses Skript) -> FadeLayer (CanvasLayer) -> FadeRect (ColorRect)
		_fadeLayer = GetNode<CanvasLayer>("FadeLayer");
		_fadeRect = GetNode<ColorRect>("FadeLayer/FadeRect");
	}

	public async void GotoScene(string sceneKey, string spawnPoint = "")
	{
		if (!_scenes.ContainsKey(sceneKey))
		{
			GD.PrintErr($"SceneManager: Unbekannte Szene '{sceneKey}'");
			return;
		}

		_nextSpawnPoint = spawnPoint;

		await Fade(1.0f);
		GetTree().ChangeSceneToFile(_scenes[sceneKey]);

		// Einen Frame warten, damit die neue Szene _Ready() durchlaufen kann,
		// bevor wieder eingeblendet wird.
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await Fade(0.0f);
	}

	private async Task Fade(float targetAlpha)
	{
		var tween = CreateTween();
		tween.TweenProperty(_fadeRect, "color:a", targetAlpha, 0.4f);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	public string GetSpawnPoint()
	{
		// Wird von der neuen Szene in _Ready() aufgerufen, um zu wissen,
		// wo der Spieler auftauchen soll. Wird danach zurückgesetzt.
		var point = _nextSpawnPoint;
		_nextSpawnPoint = "";
		return point;
	}
}
