using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Magicolo.GeneralTools {
	public static class SpawnerCustomMenus {
	
		[MenuItem("Magicolo's Tools/Create/Spawner")]
		static void CreateSpawner() {
			GameObject gameObject;
			Spawner existingSpawner = Object.FindObjectOfType<Spawner>();
		
			if (existingSpawner == null) {
				gameObject = new GameObject();
				gameObject.name = "Spawner";
				gameObject.AddComponent<Spawner>();
				Undo.RegisterCreatedObjectUndo(gameObject, "Spawner Created");
			}
			else {
				gameObject = existingSpawner.gameObject;
			}
			
			Selection.activeGameObject = gameObject;
		}
	}
}