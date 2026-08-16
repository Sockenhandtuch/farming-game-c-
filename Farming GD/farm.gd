extends Node2D
## Wurzel-Skript der FARM-Szene.
## Zeigt das Gegenstück zu shop_door.gd: Wie eine Szene beim Start
## herausfindet, WO der Spieler stehen soll (z.B. wenn er aus dem
## Dungeon zurückkommt, soll er nicht am Kartenrand, sondern am
## Dungeon-Ausgang landen).

@onready var player: CharacterBody2D = $Player

# Alle möglichen Spawn-Punkte in dieser Szene, benannt über Marker2D-Nodes
# im Szenenbaum. So kannst du im Editor beliebig viele Punkte platzieren,
# ohne Code anzufassen.
@onready var spawn_points: Dictionary = {
	"default": $SpawnPoints/Default,
	"shop_exit": $SpawnPoints/ShopExit,
	"dungeon_exit": $SpawnPoints/DungeonExit,
}


func _ready() -> void:
	var spawn_key := SceneManager.get_spawn_point()
	var target_point: Node2D = spawn_points.get(spawn_key, spawn_points["default"])
	player.global_position = target_point.global_position

	# UI z.B. für Gold-Anzeige an GameState-Signal hängen,
	# statt Gold-Wert manuell zu kopieren
	GameState.gold_changed.connect(_on_gold_changed)
	_on_gold_changed(GameState.gold)


func _on_gold_changed(new_amount: int) -> void:
	# Hier würdest du z.B. ein Label in der UI aktualisieren
	pass
