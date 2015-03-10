using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class SpawnerHierarchyManager {

		public Spawner spawner;
		
		public SpawnerHierarchyManager(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void Initialize(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void FreezeTransforms() {
			spawner.transform.hideFlags = HideFlags.HideInInspector;
			spawner.transform.Reset();
		}
	}
}