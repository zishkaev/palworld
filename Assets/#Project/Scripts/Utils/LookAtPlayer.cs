using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
	public bool justY = true;
	public Vector3 Offset;
	Transform cam;

	private void Start() {
		cam = PlayerController.instance.transform;
		if (!cam)
			this.enabled = false;
	}

	void Update() {
		var lookPos = cam.position - transform.position;
		lookPos.y = 0;
		if (lookPos != Vector3.zero) {
			var rotation = Quaternion.LookRotation(lookPos);

			transform.eulerAngles = (new Vector3(justY ? 0 : rotation.eulerAngles.x, rotation.eulerAngles.y, 0) + Offset);
		}
	}
}
