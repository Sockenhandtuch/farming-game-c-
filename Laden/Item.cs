using Godot;
using System.Collections.Generic;

// Eine "Ware", die der Spieler anbauen/finden und verkaufen kann.
// Bewusst als Resource statt normale Klasse, damit du im Godot-Editor
// Items als .tres-Dateien anlegen kannst (Rechtsklick > Neue Resource)
// statt jedes Item im Code hart zu verdrahten.
[GlobalClass]
public partial class Item : Resource
{
	[Export] public string Id { get; set; } = "";
	[Export] public string DisplayName { get; set; } = "";
	[Export] public int BasePrice { get; set; } = 10;

	// Tags beschreiben, WOFÜR das Item gut ist. Ein Kaffee hat z.B.
	// die Tags ["wachmacher", "warm", "bitter"]. Je mehr Tags mit dem
	// Kundenbedürfnis übereinstimmen, desto besser der Preis.
	[Export] public string[] Tags { get; set; } = System.Array.Empty<string>();

	public bool HasTag(string tag)
	{
		foreach (var t in Tags)
		{
			if (t == tag) return true;
		}
		return false;
	}
}
