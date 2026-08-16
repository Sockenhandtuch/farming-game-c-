# Shop-System in C# (Godot)

Baut auf dem vorherigen GDScript-Beispiel auf, aber die Shop-Logik ist
jetzt in C#. Godot erlaubt es, GDScript und C# im selben Projekt zu
mischen (z.B. SceneManager bleibt GDScript, Shop wird C#) - das
funktioniert problemlos über Autoloads.

## Grundidee der Rate-Mechanik

1. Ein `Item` hat **Tags** statt einer festen Kategorie
   (z.B. "Kaffee" → ["Wach macher", "Warm", "Bitter"])
2. Ein `Customer` hat ein **Bedürfnis**, das zu bestimmten Tags passt
   (z.B. "müde" → passt zu "Wachmacher")
3. Beim Verkauf wird verglichen: Wie viele Tags des Items matchen
   das Bedürfnis des Kunden? → daraus ergibt sich der Preis-Multiplikator

Das ist bewusst **tag-basiert statt fest verdrahtet** ("wenn Kunde X dann
Item Y"), weil du sonst für jede Kombination Code schreiben müsstest.
Mit Tags kannst du beliebig neue Items/Kunden hinzufügen, ohne Logik
anzufassen - nur Daten.

## Dateien

- `Item.cs` – Datenklasse für Waren mit Tags
- `Customer.cs` – Datenklasse für Kunden mit Bedürfnis + Hinweistext
- `ShopManager.cs` – Kernlogik: Kunde generieren, Preis berechnen, verkaufen
- `ShopUI.cs` – Beispiel, wie die UI an ShopManager andockt (Signale)
