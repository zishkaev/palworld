using System.Collections;
using UnityEngine;

// TODO: Looks like coroutine and Update do the same at the same time. Refactor!
public sealed class MagnetizeRigidBodyToPlayer : MonoBehaviour {
	[SerializeField] 
	private Rigidbody _body;
	[SerializeField]
	private float _acceleration, _maxSpeed, _magnetizeDistance, _delay;
	private float _currentSpeed;
	private Transform _target;
	private bool _active;
	private YieldInstruction _wait;
	private Coroutine moveToPoint;
	private Collider _collider;
	private void Awake() {
		_wait = new WaitForSeconds(_delay);
		_collider = GetComponentInChildren<Collider>();
	}
	void SetTarget(Transform target) {
		_target = target;
		_body.isKinematic = _target;
	}
	private void OnEnable() {
		SetTarget(null);
		_active = false;
		StartCoroutine(Activate());
		IEnumerator Activate() {
			yield return _wait;
			_active = true;
			_collider.isTrigger = true;
		}
	}

	Transform PlayerTransform => PlayerController.instance.mainPoint;

	private void Move(Vector3 p) {
		var delta = Time.deltaTime;
		_currentSpeed = Mathf.Min(_currentSpeed + delta * _acceleration, _maxSpeed);
		var dir = p - _body.transform.position;
		var distance = _currentSpeed * delta;
		transform.position += dir.normalized * Mathf.Min(distance, dir.magnitude);
	}

	public void SetTargetPoint(Vector3 targetPoint) {
		if (moveToPoint != null)
			StopCoroutine(moveToPoint);
		if (targetPoint.magnitude < float.Epsilon)
			return;
		moveToPoint = StartCoroutine(MoveToPoint(targetPoint));
	}

	IEnumerator MoveToPoint(Vector3 targetPoint) {
		while ((targetPoint - _body.transform.position).magnitude > 2f) {
			Move(targetPoint);
			yield return null;
			if (_target != null)
				yield break;
		}
	}

	public void Update() {
		if (!_active)
			return;
		if (_target) {
			Move(_target.position);
		}
		else {
			var distance = Vector3.Distance(_body.transform.position, PlayerTransform.position);
			if (distance <= _magnetizeDistance) {
				SetTarget(PlayerTransform);
				_currentSpeed = 0;
			}
		}
	}
}