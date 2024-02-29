using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrencyLoot", menuName = "Asset/CurrencyLoot", order = 0)]
public sealed class CurrencyLoot : ScriptableObject {
	[SerializeField]
	private GameObject _prefab;
	[SerializeField]
	private int
		_minValue, _maxValue,
		targetBallsCount = 20;
	[SerializeField]
	private float _spawnSpeed;
	[Serializable]
	sealed class Data {
		public int _maxAmount;
		public float _scale;
	}
	//	Важно!
	//	Каждый последующий элемент должен быть кратен предыдущему!
	[SerializeField]
	private List<Data> _dropSizes = new List<Data>();

	private Dictionary<int, int> _amountMultipliers;

	public void SpawnResources(Vector3 position, Vector3 targetPosition, int droppedAmount) {
		if (droppedAmount <= 0)
			return;
		//Debug.Log($"___Dropping {droppedAmount} {_prefab.GetComponent<GiveCurrencyOnCollide>()._currencyType}");
		int actualValue = 0;
		Dictionary<int, int> sizesDestribution = GetLootDestribution(droppedAmount);
		foreach (int ballSize in sizesDestribution.Keys) {
			float scale = _dropSizes[ballSize]._scale;
			int amount = _dropSizes[ballSize]._maxAmount;
			for (int i = sizesDestribution[ballSize]; i > 0; --i) {
				SpawnBall(position, targetPosition, scale, amount);
				actualValue += amount;
			}
		}
		Debug.Log($"Actual value: {actualValue}");
	}
	public void SpawnLoot(Vector3 position) {
		int randomedAmount = RandomUtils.Range(_minValue, _maxValue);
		SpawnResources(position, Vector3.zero, randomedAmount);
	}

	public void SpawnLoot(Vector3 position, Vector3 targetPosition) {
		int randomedAmount = RandomUtils.Range(_minValue, _maxValue);
		SpawnResources(position, targetPosition, randomedAmount);
	}

	private void SpawnBall(Vector3 position, Vector3 targetPosition, float scale, int amount) {
		GameObject theBall = Instantiate(_prefab, position, Quaternion.identity);
		theBall.transform.localScale = scale * Vector3.one;
		theBall.transform.rotation = Quaternion.Euler(RandomUtils.RandomV3 * 360f);
		if (theBall.TryGetComponent(out Rigidbody rb)) {
			rb.velocity = RandomUtils.RandomV3Half.normalized * _spawnSpeed;
			rb.AddTorque(RandomUtils.RandomV3 * _spawnSpeed, ForceMode.Impulse);
		}
		if (theBall.TryGetComponent(out GiveCurrencyOnCollide currency))
			currency._amount = amount;
		if (theBall.TryGetComponent(out MagnetizeRigidBodyToPlayer magnetize))
			magnetize.SetTargetPoint(targetPosition);
	}

	private Dictionary<int, int> GetLootDestribution(int amount) {
		Dictionary<int, int> result = new Dictionary<int, int>();
		int ballsCount = 0;
		// Собирается оптимальный (минимальный) набор
		for (int i = _dropSizes.Count - 1; i >= 0; --i)
			if (_dropSizes[i]._maxAmount <= amount) {
				result[i] = amount / _dropSizes[i]._maxAmount;
				amount -= result[i] * _dropSizes[i]._maxAmount;
				ballsCount += result[i];
			}
		// Если набор не собирается, то сумма будет чуть больше
		if (amount > 0) {
			if (result.ContainsKey(0))
				++result[0];
			else
				result[0] = 1;
			++ballsCount;
		}

		CountAmountMultipliers();

		//	Шаров мешьше чем хотелось бы	и	их всё ещё можно дробить
		while (ballsCount < targetBallsCount && (result.Count > 1 || !result.ContainsKey(0))) {
			List<int> currentSizes = new List<int>(result.Keys.ToArray());
			currentSizes.Remove(0);

			int randomSize = currentSizes[RandomUtils.RandomIndex(currentSizes.Count)];

			if (result[randomSize] == 1)
				result.Remove(randomSize);
			else
				--result[randomSize];

			if (result.ContainsKey(randomSize - 1))
				result[randomSize - 1] += _amountMultipliers[randomSize];
			else
				result[randomSize - 1] = _amountMultipliers[randomSize];

			ballsCount += _amountMultipliers[randomSize] - 1;
		}

		return result;
	}

	private void CountAmountMultipliers() {
		if (_amountMultipliers != null) return;
		_amountMultipliers = new Dictionary<int, int>();

		for (int i = _dropSizes.Count - 1; i > 0; --i)
			_amountMultipliers[i] = (_dropSizes[i]._maxAmount / _dropSizes[i - 1]._maxAmount);
	}
}