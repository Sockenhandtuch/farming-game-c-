# Szenen-System: Farm / Shop / Dungeon

Dieses Beispiel zeigt eine gängige Godot-Architektur, um zwischen mehreren
"Spiel-Modi" (Farm, Shop, Sidescroller-Dungeon) zu wechseln, **ohne** dass
die Szenen sich gegenseitig direkt kennen müssen.

## Grundprinzip: Autoload-SceneManager

Der Trick ist ein **Autoload (Singleton)**, der als "Schaltzentrale" zwischen
den Szenen fungiert. Jede einzelne Szene (Farm.tscn, Shop.tscn, Dungeon.tscn)
weiß nichts von den anderen – sie ruft nur `SceneManager.goto("shop")` auf
und muss sich um nichts weiter kümmern.

Vorteile für dich als Solo-Entwickler:
- Jede Szene kann isoliert entwickelt und getestet werden (du kannst die
  Shop-Szene öffnen und testen, ohne die Farm geladen zu haben)
- Der Spielstand (Gold, Inventar, Ernte-Fortschritt) bleibt beim Wechsel
  erhalten, weil er NICHT in den Szenen selbst liegt, sondern in einem
  zweiten Autoload: `GameState`
- Übergänge (Fade-In/Out) lassen sich zentral an einer Stelle bauen

## Datei-Übersicht

- `scene_manager.gd` – Autoload, lädt/wechselt Szenen, macht Fade-Transitions
- `game_state.gd` – Autoload, hält den Spielstand (Gold, Inventar, aktueller Tag)
- `farm.gd` – Beispiel-Skript für die Farm-Szene
- `shop_door.gd` – Beispiel: interagierbares Objekt, das zum Shop wechselt
- `dungeon_entrance.gd` – Beispiel: Dungeon-Eingang auf der Insel

## Setup in Godot

1. Projekt > Projekteinstellungen > Autoload
2. `scene_manager.gd` als "SceneManager" hinzufügen
3. `game_state.gd` als "GameState" hinzufügen
4. Fertig – ab jetzt sind beide von JEDER Szene aus erreichbar,
   ohne sie zu importieren
