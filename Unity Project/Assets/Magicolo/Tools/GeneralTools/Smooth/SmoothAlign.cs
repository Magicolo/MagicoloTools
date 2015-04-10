using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	[AddComponentMenu("Magicolo/Smooth/Align")]
	public class SmoothAlign : MonoBehaviourExtended {

		[Mask(Axis.XYZ)] public Axis axis = Axis.XYZ;
		public Transform target;
		public Vector3 offset;
		[Clamp(0, 100)] public Vector3 damping = new Vector3(100, 100, 100);
	
		void Update() {
			Vector3 eulerAngles = transform.eulerAngles;
			
			eulerAngles.x = axis.Contains(Axis.X) ? Mathf.LerpAngle(eulerAngles.x, target.eulerAngles.x + offset.x, damping.x * Time.deltaTime) : eulerAngles.x;
			eulerAngles.y = axis.Contains(Axis.Y) ? Mathf.LerpAngle(eulerAngles.y, target.eulerAngles.y + offset.y, damping.y * Time.deltaTime) : eulerAngles.y;
			eulerAngles.z = axis.Contains(Axis.Z) ? Mathf.LerpAngle(eulerAngles.z, target.eulerAngles.z + offset.z, damping.z * Time.deltaTime) : eulerAngles.z;
			
			transform.eulerAngles = eulerAngles;
		}
	}
}
