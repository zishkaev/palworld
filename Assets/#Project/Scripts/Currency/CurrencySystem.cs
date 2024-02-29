using System;
using UnityEngine;

public class CurrencySystem : Singletone<CurrencySystem> {
	[SerializeField] private int currentValue;
	public Action OnChangeValue;
	public int CurrentValue {
		get { return currentValue; }
		set {
			currentValue = value;
			OnChangeValue?.Invoke();
		}
	}

	public void Add(int value) {
		CurrentValue += value;
	}

	public bool CanBuy(int price) => CurrentValue > price;

	public bool Spend(int price) {
		if (CanBuy(price)) {
			CurrentValue -= price;
			return true;
		}
		return false;
	}
}
