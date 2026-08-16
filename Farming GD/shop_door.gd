extends Area2D
## Wird an ein interagierbares Objekt in der FARM-Szene gehängt
## (z.B. die Tür des eigenen Shops). Wenn der Spieler reinläuft
## und eine Taste drückt, wechseln wir zur Shop-Szene.

var player_in_range: bool = false


func _ready() -> void:
	# Area2D-Signale verbinden: erkennt, ob der Spieler-Körper reinläuft
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)


func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		player_in_range = true
		# Hier könntest du z.B. ein "[E] Shop betreten"-Prompt einblenden


func _on_body_exited(body: Node2D) -> void:
	if body.is_in_group("player"):
		player_in_range = false


func _unhandled_input(event: InputEvent) -> void:
	if player_in_range and event.is_action_pressed("interact"):
		# "shop_entrance" sagt der Shop-Szene, wo der Spieler auftauchen soll
		# (z.B. direkt hinter der Ladentür, nicht am Kartenrand)
		SceneManager.goto_scene("shop", "shop_entrance")
