extends Node
## Autoload-Singleton: "GameState"
##
## Hält ALLES, was über einen Szenenwechsel hinweg erhalten bleiben muss.
## Wichtig: Die Farm-Szene, Shop-Szene etc. speichern NICHTS Wichtiges
## in sich selbst - sie lesen/schreiben nur in GameState. Sonst würde
## z.B. dein Gold beim Betreten des Dungeons einfach verschwinden,
## weil die Farm-Szene (und ihre Variablen) entladen wird.

signal gold_changed(new_amount: int)
signal inventory_changed()

var gold: int = 100
var current_day: int = 1

# Inventar als Dictionary: { "kartoffel": 5, "erz": 2, ... }
# Einfacher als eine Klasse pro Item - reicht für den Start völlig.
var inventory: Dictionary = {}


func add_item(item_id: String, amount: int = 1) -> void:
	inventory[item_id] = inventory.get(item_id, 0) + amount
	inventory_changed.emit()


func remove_item(item_id: String, amount: int = 1) -> bool:
	# Gibt false zurück, falls nicht genug vorhanden ist -
	# so kann der aufrufende Code (z.B. der Shop) das sauber abfangen.
	if inventory.get(item_id, 0) < amount:
		return false
	inventory[item_id] -= amount
	if inventory[item_id] <= 0:
		inventory.erase(item_id)
	inventory_changed.emit()
	return true


func add_gold(amount: int) -> void:
	gold += amount
	gold_changed.emit(gold)


func spend_gold(amount: int) -> bool:
	if gold < amount:
		return false
	gold -= amount
	gold_changed.emit(gold)
	return true


func advance_day() -> void:
	current_day += 1
	# Hier später: Pflanzenwachstum aktualisieren, Kunden-Pool neu würfeln, etc.
