using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class SphereCastAllSettings {
		
		public Vector3 offset;
		public float radius = 1;
		public LayerMask layerMask;
		
		public bool HasHit(Vector3 origin, bool debug = false) {
			return GetHits(origin, debug).Length > 0;
		}
		
		public RaycastHit[] GetHits(Vector3 origin, bool debug = false) {
			RaycastHit[] hits = Physics.SphereCastAll(origin + offset, radius, Vector3.forward, Mathf.Infinity, layerMask);
			
			if (debug) {
				DrawRays(origin);
			}
			
			return hits;
		}
		
		public void DrawRays(Vector3 origin) {
			DrawRays(origin, 16);
		}
		
		public void DrawRays(Vector3 origin, int amountOfRays) {
			for (int i = 0; i < amountOfRays; i++) {
				Debug.DrawRay(origin + offset, Vector2.up.Rotate(i * (360 / amountOfRays)) * radius, Color.yellow);
			}
		}
	}
	
	[System.Serializable]
	public class GroundCastSettings {
		
		public Vector3 offset;
		[Range(-90, 90)] public float spread = 30;
		[Min] public float distance = 1;
		public LayerMask layerMask;
		
		public bool HasHit(Vector3 origin, Vector3 direction, bool debug = false) {
			return GetHits(origin, direction, debug).Length > 0;
		}
		
		public RaycastHit[] GetHits(Vector3 origin, Vector3 direction, bool debug = false) {
			List<RaycastHit> hits = new List<RaycastHit>();
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
		
			RaycastHit hit;
			if (Physics.Raycast(origin + offset, direction, out hit, distance, layerMask)) {
				hits.Add(hit);
			}
			
			if (Physics.Raycast(origin + offset, direction.Rotate(spread), out hit, adjustedDistance, layerMask)) {
				hits.Add(hit);
			}
			
			if (Physics.Raycast(origin + offset, direction.Rotate(-spread), out hit, adjustedDistance, layerMask)) {
				hits.Add(hit);
			}
		
			if (debug) {
				DrawRays(origin, direction);
			}
		
			return hits.ToArray();
		}
		
		public void DrawRays(Vector3 origin, Vector3 direction) {
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
			
			Debug.DrawRay(origin + offset, direction * distance, Color.green);
			Debug.DrawRay(origin + offset, direction.Rotate(spread) * adjustedDistance, Color.green);
			Debug.DrawRay(origin + offset, direction.Rotate(-spread) * adjustedDistance, Color.green);
		}
	}
}
