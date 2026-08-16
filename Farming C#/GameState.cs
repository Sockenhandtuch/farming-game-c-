using Godot;
using System.Collections.Generic;

// Autoload-Singleton: "GameState"
//
// Hält ALLES, was über einen Szenenwechsel hinweg erhalten bleiben muss:
// Gold, Inventar, aktueller Tag. Die Farm-/Shop-/Dungeon-Szenen speichern
// selbst nichts Wichtiges - sie lesen/schreiben nur hier.
public partial class GameState : Node
{
	public static GameState Instance { get; private set; }

	[Signal] public delegate void GoldChangedEventHandler(int newAmount);
	[Signal] public delegate void InventoryChangedEventHandler();

	public int Gold { get; private set; } = 100;
	public int CurrentDay { get; private set; } = 1;

	// Welcher Dungeon zuletzt betreten wurde - wird z.B. von
	// DungeonEntrance.cs gesetzt, bevor zur Dungeon-Szene gewechselt wird.
	public string CurrentDungeonId { get; set; } = "";

	// Inventar als Dictionary: "kartoffel" -> 5, "erz" -> 2, ...
	private readonly Dictionary<string, int> _inventory = new();

	public override void _Ready()
	{
		Instance = this;
	}

	public void AddItem(string itemId, int amount = 1)
	{
		_inventory.TryGetValue(itemId, out int current);
		_inventory[itemId] = current + amount;
		EmitSignal(SignalName.InventoryChanged);
	}

	// Gibt false zurück, falls nicht genug vorhanden ist - so kann
	// der aufrufende Code (z.B. der Shop) das sauber abfangen.
	public bool RemoveItem(string itemId, int amount = 1)
	{
		_inventory.TryGetValue(itemId, out int current);
		if (current < amount)
			return false;

		int remaining = current - amount;
		if (remaining <= 0)
			_inventory.Remove(itemId);
		else
			_inventory[itemId] = remaining;

		EmitSignal(SignalName.InventoryChanged);
		return true;
	}

	public int GetItemCount(string itemId)
	{
		_inventory.TryGetValue(itemId, out int current);
		return current;
	}

	public void AddGold(int amount)
	{
		Gold += amount;
		EmitSignal(SignalName.GoldChanged, Gold);
	}

	public bool SpendGold(int amount)
	{
		if (Gold < amount)
			return false;

		Gold -= amount;
		EmitSignal(SignalName.GoldChanged, Gold);
		return true;
	}

	public void AdvanceDay()
	{
		CurrentDay++;
		// Hier später: Pflanzenwachstum aktualisieren, Kunden-Pool neu würfeln, etc.
	}
}
