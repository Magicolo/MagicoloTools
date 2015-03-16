using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class SpawnerPoolManager {

		public SpawnerPool[] pools = new SpawnerPool[0];
		public Spawner spawner;
    	
		Dictionary<string, SpawnerPool> poolDict;
		
		public SpawnerPoolManager(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void Initialize(Spawner spawner) {
			this.spawner = spawner;
		}
    	
		public void Start() {
			foreach (SpawnerPool pool in pools) {
				pool.Start();
			}
		}
		
		public void BuildPoolDict() {
			poolDict = new Dictionary<string, SpawnerPool>();
			
			foreach (SpawnerPool pool in pools) {
				poolDict[pool.Name] = pool;
			}
		}
		
		public SpawnerPool GetPool(string poolName) {
			SpawnerPool pool = null;
			
			try {
				pool = poolDict[poolName];
			}
			catch {
				Logger.LogError(string.Format("Prefab named {0} was not found.", poolName));
			}
			
			return pool;
		}
	}
}
