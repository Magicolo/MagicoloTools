using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GraphicsTools {
	[System.Serializable]
	public class ModelPhysicsOptions {

		public enum ColliderTypes {
			Sphere,
			Box,
			Circle2D,
			Box2D,
			Polygon2D
		}
	
		public ColliderTypes colliderType;
		[Min] public float colliderSize = 1;
		public bool generateColliders = true;
		public List<string> deactivatedColliders = new List<string>();
		public bool initialized;
	}
}

