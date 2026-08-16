using Godot;
using System.Collections.Generic;

// Zentrale Logik des Shops. Wird als Node in der Shop-Szene platziert
// (kein Autoload nötig, da Shop-Logik nur relevant ist, während man
// im Shop ist).
public partial class ShopManager : Node
{
	// Signale, an die sich die UI hängt - ShopManager weiß nichts
	// über Labels/Buttons, nur über Daten. Trennung von Logik und Darstellung.
	[Signal] public delegate void CustomerArrivedEventHandler(Customer customer);
	[Signal] public delegate void SaleResultEventHandler(int finalPrice, float matchQuality);

	// Alle im Spiel verfügbaren Kunden-Typen, im Editor als Resourcen
	// zuweisbar statt im Code hart zu verdrahten.
	[Export] public Customer[] PossibleCustomers { get; set; } = System.Array.Empty<Customer>();

	private RandomNumberGenerator _rng = new();
	private Customer _currentCustomer;

	public override void _Ready()
	{
		_rng.Randomize();
	}

	// Wird z.B. von einem "Nächster Kunde"-Button oder Timer aufgerufen
	public void SpawnNextCustomer()
	{
		if (PossibleCustomers.Length == 0)
		{
			GD.PrintErr("ShopManager: Keine Kunden konfiguriert.");
			return;
		}

		int index = _rng.RandiRange(0, PossibleCustomers.Length - 1);
		_currentCustomer = PossibleCustomers[index];
		EmitSignal(SignalName.CustomerArrived, _currentCustomer);
	}

	// Wird aufgerufen, wenn der Spieler ein Item anbietet.
	// Gibt den finalen Preis zurück und wie gut das Match war (0.0 - 1.0),
	// damit die UI z.B. "Treffer!" oder "naja..." anzeigen kann.
	public (int finalPrice, float matchQuality) OfferItem(Item item)
	{
		if (_currentCustomer == null)
		{
			GD.PrintErr("ShopManager: Kein aktiver Kunde.");
			return (0, 0f);
		}

		float matchQuality = CalculateMatchQuality(item, _currentCustomer);
		int finalPrice = CalculatePrice(item.BasePrice, matchQuality);

		EmitSignal(SignalName.SaleResult, finalPrice, matchQuality);

		// Kunde ist bedient - für den nächsten Verkauf muss SpawnNextCustomer
		// erneut aufgerufen werden. So verhindern wir versehentliche Doppelverkäufe.
		_currentCustomer = null;

		return (finalPrice, matchQuality);
	}

	private float CalculateMatchQuality(Item item, Customer customer)
	{
		// Einfachster Fall: Item hat das gesuchte Tag oder nicht.
		// matchQuality als float (nicht bool) lässt später Erweiterung zu,
		// z.B. Teil-Matches über mehrere Bedürfnis-Tags statt nur einem.
		return item.HasTag(customer.NeedTag) ? 1.0f : 0.0f;
	}

	private int CalculatePrice(int basePrice, float matchQuality)
	{
		// Schlechtes Match: Preis sinkt deutlich (Kunde ist unzufrieden)
		// Gutes Match: Preis steigt über den Basispreis (Kunde zahlt gern mehr)
		// Diese Kurve ist ein erster Entwurf - hier lohnt sich später Balancing.
		float multiplier = Mathf.Lerp(0.4f, 1.6f, matchQuality);
		return Mathf.RoundToInt(basePrice * multiplier);
	}
}
