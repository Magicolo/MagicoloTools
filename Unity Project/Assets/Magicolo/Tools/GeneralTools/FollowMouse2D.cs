using UnityEngine;
using Magicolo;

namespace Magicolo {
	[AddComponentMenu("Magicolo/Follow Mouse 2D")]
	public class FollowMouse2D : MonoBehaviourExtended {

		public KeyCode[] keys = { KeyCode.Mouse0 };
		public bool continuous;
		public bool alwaysFaceMouse;
		public float moveSpeed = 3;
		public float turnSpeed = 10;
	
		[HideInInspector] public Vector3 targetPosition;
	
		Camera mainCamera;

		void Start() {
			mainCamera = Camera.main;
			targetPosition = transform.position;
		}
	
		void Update() {
			SetTargetPosition();
			Move();
		}
			
		void SetTargetPosition() {
			foreach (KeyCode key in keys) {
				if (continuous) {
					if (Input.GetKey(key)) {
						targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
						targetPosition.z = transform.position.z;
					}
				}
				else if (Input.GetKeyDown(key)) {
					targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
					targetPosition.z = transform.position.z;
				}
			}
		}
				
		void Move() {
			if ((targetPosition - transform.position).magnitude > moveSpeed * Time.deltaTime) {
				transform.GetComponent<Rigidbody2D>().MovePosition(transform.position + (targetPosition - transform.position).normalized * moveSpeed * Time.deltaTime);
				transform.GetComponent<Rigidbody2D>().MoveRotation(transform.LookingAt2D(targetPosition, 0, turnSpeed).eulerAngles.z);
			}
			else transform.position = targetPosition;
			
			if (alwaysFaceMouse) {
				Vector3 targetDirection = mainCamera.ScreenToWorldPoint(Input.mousePosition);
				targetDirection.z = transform.position.z;
				transform.GetComponent<Rigidbody2D>().MoveRotation(transform.LookingAt2D(targetDirection, 0, turnSpeed).eulerAngles.z);
			}
		}
	}
}