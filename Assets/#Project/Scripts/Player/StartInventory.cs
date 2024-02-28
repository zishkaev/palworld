using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInventory : Singletone<StartInventory> {
	public List<Item> startItems = new List<Item>();
	public List<Item> boughItems = new List<Item>();

	public void BoughItem(Item item) {
		boughItems.Add(item);
	}
}
