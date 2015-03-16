using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class SpawnerPrefab {

		public string Name {
			get {
				return prefab.name;
			}
		}

		public GameObject prefab;
		public Spawner spawner;
		
		public SpawnerPrefab(GameObject prefab) {
			this.prefab = prefab;
		}
		
		public void Initialize(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void Start() {
			
		}
		
		public void ResetToInitialValues(GameObject gameObject) {
			
		}
	}
}
