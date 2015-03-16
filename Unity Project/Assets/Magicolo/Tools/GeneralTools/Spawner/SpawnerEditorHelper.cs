using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[System.Serializable]
	public class SpawnerEditorHelper : EditorHelper {

		public Spawner spawner;
		
		public SpawnerEditorHelper(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void Initialize(Spawner spawner) {
			this.spawner = spawner;
		}
		
		public void RepaintInspector() {
			#if UNITY_EDITOR
			UnityEditor.SerializedObject poolSerialized = new UnityEditor.SerializedObject(spawner);
			UnityEditor.SerializedProperty repaintDummyProperty = poolSerialized.FindProperty("editorHelper").FindPropertyRelative("repaintDummy");
			repaintDummyProperty.SetValue(!repaintDummy);
			#endif
		}
	}
}
