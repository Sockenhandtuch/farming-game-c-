using Godot;

// Ein Kunde, der ein Bedürfnis hat, aber es nur ANDEUTET statt
// direkt zu sagen, was er will. "NeedTag" ist die "Lösung" im Hintergrund,
// "HintText" ist das, was der Spieler tatsächlich zu lesen bekommt.
[GlobalClass]
public partial class Customer : Resource
{
	[Export] public string DisplayName { get; set; } = "";

	// z.B. "wachmacher" - wird intern zum Abgleich mit Item-Tags benutzt,
	// taucht aber NIE direkt im UI auf (sonst wäre das Rateelement weg)
	[Export] public string NeedTag { get; set; } = "";

	// Mehrere mögliche Hinweistexte pro Bedürfnis, damit es sich nicht
	// wiederholt anfühlt. Einer wird zufällig gewählt.
	[Export] public string[] HintTexts { get; set; } = System.Array.Empty<string>();

	public string GetRandomHint(RandomNumberGenerator rng)
	{
		if (HintTexts.Length == 0) return "...";
		int index = rng.RandiRange(0, HintTexts.Length - 1);
		return HintTexts[index];
	}
}
