using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]  
	public class SpawnerPool {

		public string Name {
			get {
				return prefab.Name;
			}
		}
		
		public bool resetToInitialValues;
		public SpawnerPrefab prefab;
		public Spawner spawner;
		
		public SpawnerPool(GameObject prefab, Spawner spawner) {
			this.prefab = new SpawnerPrefab(prefab);
			this.spawner = spawner;
		}
		
		public void Initialize(Spawner spawner) {
			this.spawner = spawner;
		}

		public void Start() {
			prefab.Start();
		}
	}
}
