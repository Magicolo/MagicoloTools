using UnityEngine;

namespace Magicolo.GeneralTools {
	[ExecuteInEditMode, AddComponentMenu("Magicolo/Smooth Follow 2D")]
	public class SmoothFollow2D : MonoBehaviour {

		[Tooltip("Object to follow.")]
		public Transform target;
		[Tooltip("Position offset.")]
		public Vector3 offset = new Vector3(0, 0, -10);
		[Tooltip("Strength of the lerp.")]
		[Clamp(0, 100)] public Vector3 damping = new Vector3(100, 100, 100);
	
		void Update() {
			if (!Application.isPlaying && target) {
				transform.position = target.position + offset;
			}
			else if (Application.isPlaying && target) {
				Vector3 position = transform.position;
			
				position.x = Mathf.Lerp(position.x, target.position.x + offset.x, damping.x * Time.deltaTime);
				position.y = Mathf.Lerp(position.y, target.position.y + offset.y, damping.y * Time.deltaTime);
				position.z = Mathf.Lerp(position.z, target.position.z + offset.z, damping.z * Time.deltaTime);
			
				transform.position = position;
			}
		}
	}
}
			