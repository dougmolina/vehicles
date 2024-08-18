using UnityEngine;

namespace _Vehicles.Scripts.V1 {
	public class Wheel : MonoBehaviour {
		[Header("Suspension")]
		[SerializeField]
		private float _radius;
		[SerializeField]
		private float _restPosition;
		[SerializeField]
		private float _suspensionDistance;
		[SerializeField]
		private float _springStiffness;
		[SerializeField]
		private float _damperStiffness;

		private float _minLength;
		private float _maxLength;
		private float _springLength;
		private float _springLastLength;
		private float _springVelocity;
		private float _springForce;
		private float _damperForce;

		private Rigidbody _rigidBody;
		private Vector3 _suspensionForce;

		private void Start() {
			_rigidBody = transform.parent.GetComponent<Rigidbody>();

			_minLength = _restPosition - _suspensionDistance;
			_maxLength = _restPosition + _suspensionDistance;
		}

		private void FixedUpdate() {
			if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength + _radius)) {
				_springLastLength = _springLength;
				_springLength = Mathf.Clamp(hit.distance - _radius, _minLength, _maxLength);
				_springVelocity = (_springLastLength - _springLength) / Time.fixedDeltaTime;
				_springForce = _springStiffness * (_restPosition - _springLength);
				_damperForce = _damperStiffness * _springVelocity;
				_suspensionForce = (_springForce + _damperForce) * transform.up;

				_rigidBody.AddForceAtPosition(_suspensionForce, hit.point);
			}
		}
	}
}
