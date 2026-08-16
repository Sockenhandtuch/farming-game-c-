using Godot;

// Zeigt beispielhaft, wie die UI an ShopManager andockt, OHNE dass
// ShopManager irgendetwas über Labels oder Buttons wissen muss.
// Diese Trennung heißt: Du kannst später die komplette UI austauschen
// (anderes Layout, Animationen etc.), ohne ShopManager anzufassen.
public partial class ShopUI : Control
{
	[Export] public ShopManager Shop { get; set; }
	[Export] public Label HintLabel { get; set; }
	[Export] public Label ResultLabel { get; set; }

	public override void _Ready()
	{
		// Godots C#-Signal-Syntax: += statt .connect()
		Shop.CustomerArrived += OnCustomerArrived;
		Shop.SaleResult += OnSaleResult;

		Shop.SpawnNextCustomer();
	}

	private void OnCustomerArrived(Customer customer)
	{
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		HintLabel.Text = $"{customer.DisplayName}: \"{customer.GetRandomHint(rng)}\"";
		ResultLabel.Text = "";
	}

	private void OnSaleResult(int finalPrice, float matchQuality)
	{
		string feedback = matchQuality >= 1.0f ? "Genau richtig!" : "Naja, geht so...";
		ResultLabel.Text = $"{feedback} Verkauft für {finalPrice} Gold.";
	}

	// Wird z.B. an einen Item-Button im Inventar-UI gehängt:
	// GetNode<Button>("KaffeeButton").Pressed += () => OnItemOffered(kaffeeItem);
	public void OnItemOffered(Item item)
	{
		Shop.OfferItem(item);

		// Kurze Pause, dann nächster Kunde - hier später durch Timer/Tween ersetzen
		Shop.SpawnNextCustomer();
	}
}
