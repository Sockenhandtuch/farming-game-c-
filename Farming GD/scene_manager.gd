extends Node
## Autoload-Singleton: "SceneManager"
##
## Zentrale Stelle für alle Szenenwechsel im Spiel.
## Andere Skripte rufen einfach SceneManager.goto_scene("shop") auf
## und müssen sich nicht darum kümmern, WIE der Wechsel technisch passiert.

# Pfade zu den drei Hauptszenen - an einer Stelle gepflegt.
# Wenn du eine Szene umbenennst, änderst du es nur hier.
const SCENES := {
	"farm": "res://scenes/farm.tscn",
	"shop": "res://scenes/shop.tscn",
	"dungeon": "res://scenes/dungeon.tscn",
}

# Merkt sich, an welcher Position der Spieler in der aktuellen Szene
# stehen soll, wenn er zurückkehrt (z.B. vor der Shop-Tür, nicht am
# Kartenrand). Wird von der Zielszene ausgelesen.
var next_spawn_point: String = ""

@onready var fade_layer: CanvasLayer = $FadeLayer
@onready var fade_rect: ColorRect = $FadeLayer/FadeRect


func goto_scene(scene_key: String, spawn_point: String = "") -> void:
	# scene_key ist z.B. "shop" - kein Dateipfad, damit man sich nicht
	# vertippen kann. Falls der Key nicht existiert, brich klar ab
	# statt einen kryptischen Fehler zu werfen.
	if not SCENES.has(scene_key):
		push_error("SceneManager: Unbekannte Szene '%s'" % scene_key)
		return

	next_spawn_point = spawn_point

	# Fade-Out abspielen, DANN erst die Szene wechseln.
	# So sieht der Spieler keinen harten Schnitt.
	await _fade(1.0)
	get_tree().change_scene_to_file(SCENES[scene_key])

	# Kurz warten, damit die neue Szene ihren _ready() durchlaufen kann,
	# bevor wir wieder einblenden.
	await get_tree().process_frame
	await _fade(0.0)


func _fade(target_alpha: float) -> void:
	var tween := create_tween()
	tween.tween_property(fade_rect, "color:a", target_alpha, 0.4)
	await tween.finished


func get_spawn_point() -> String:
	# Wird von der neuen Szene in _ready() aufgerufen, um zu wissen,
	# wo der Spieler auftauchen soll (z.B. "shop_entrance" statt Standard-Spawn)
	var point := next_spawn_point
	next_spawn_point = ""  # zurücksetzen, damit es beim nächsten Wechsel neu gesetzt wird
	return point
