extends Area2D
## Analog zu shop_door.gd, aber für den Dungeon-Eingang.
## Zeigt, dass das gleiche Muster für beliebig viele Übergänge
## wiederverwendbar ist - Shop, Dungeon, später vielleicht ein Hafen
## für eine zweite Insel, alles nach demselben Schema.

@export var dungeon_id: String = "dungeon_1"  # falls du später mehrere Dungeons hast

var player_in_range: bool = false


func _ready() -> void:
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		player_in_range = true


func _on_body_exited(body: Node2D) -> void:
	if body.is_in_group("player"):
		player_in_range = false


func _unhandled_input(event: InputEvent) -> void:
	if player_in_range and event.is_action_pressed("interact"):
		# GameState merkt sich, welcher Dungeon geladen werden soll -
		# so kann dungeon.tscn beim Start prüfen, welches Layout/Material
		# es generieren bzw. anzeigen muss.
		GameState.current_dungeon_id = dungeon_id
		SceneManager.goto_scene("dungeon", "dungeon_start")
