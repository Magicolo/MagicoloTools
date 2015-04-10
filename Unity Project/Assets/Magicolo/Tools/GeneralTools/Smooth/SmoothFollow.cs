using UnityEngine;

namespace Magicolo {
	[AddComponentMenu("Magicolo/Smooth/Follow")]
	public class SmoothFollow : MonoBehaviourExtended {

		[Mask(Axis.XYZ)] public Axis axis = Axis.XYZ;
		public Transform target;
		public Vector3 offset;
		[Clamp(0, 100)] public Vector3 damping = new Vector3(100, 100, 100);
	
		void FixedUpdate() {
			Vector3 position = transform.position;
			
			position.x = axis.Contains(Axis.X) ? Mathf.Lerp(position.x, target.position.x + offset.x, damping.x * Time.fixedDeltaTime) : position.x;
			position.y = axis.Contains(Axis.Y) ? Mathf.Lerp(position.y, target.position.y + offset.y, damping.y * Time.fixedDeltaTime) : position.y;
			position.z = axis.Contains(Axis.Z) ? Mathf.Lerp(position.z, target.position.z + offset.z, damping.z * Time.fixedDeltaTime) : position.z;
			
			transform.position = position;
		}
	}
}
			