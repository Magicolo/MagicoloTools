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
	
		public bool Is3D {
			get {
				return colliderType == ColliderTypes.Sphere || colliderType == ColliderTypes.Box;
			}
		}
		
		public bool generateColliders = true;
		public ColliderTypes colliderType;
		public PhysicMaterial colliderMaterial;
		public PhysicsMaterial2D colliderMaterial2D;
		[Min] public float colliderSize = 1;
		public List<string> deactivatedColliders = new List<string>();
		public bool initialized;
	}
}

